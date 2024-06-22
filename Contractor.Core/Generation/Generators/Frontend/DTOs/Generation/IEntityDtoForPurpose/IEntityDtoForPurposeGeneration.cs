using System.Collections.Generic;
using Contractor.Core.BaseClasses;
using System.IO;
using System.Linq;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Generation.Frontend.Interfaces;
using Contractor.Core.Generation.Frontend.Model;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.DTOs
{
    public class IEntityDtoForPurposeGeneration : IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(DTOsProjectGeneration.TemplateFolder, "i-entity-kebab-dto-for-purpose.template.txt");

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoRelationAddition relationAddition;
        private readonly IEntityDtoForPurposeClassRenamer iEntityDtoForPurposeClassRenamer;
        private readonly FrontendInterfaceExtender interfaceExtender;

        public IEntityDtoForPurposeGeneration(
            EntityCoreAddition entityCoreAddition,
            FrontendDtoRelationAddition relationAddition,
            IEntityDtoForPurposeClassRenamer iEntityDtoForPurposeClassRenamer,
            FrontendInterfaceExtender interfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.relationAddition = relationAddition;
            this.iEntityDtoForPurposeClassRenamer = iEntityDtoForPurposeClassRenamer;
            this.interfaceExtender = interfaceExtender;
        }

        public void Generate(PurposeDto purposeDto)
        {
            var entitiesWithVia = DetermineEntitiesWithVia(purposeDto);
            var entitiesAlreadyGenerated = new HashSet<string>();
            var pathItemsAlreadyGenerated = new HashSet<string>();

            foreach (var purposeDtoProperty in purposeDto.Properties)
            {
                for (var index = 0; index < purposeDtoProperty.PathItems.Count; index++)
                {
                    var pathItem = purposeDtoProperty.PathItems[index];

                    bool isRoot = index == 0;
                    var withVia = !isRoot && entitiesWithVia.Any(entity => entity.Name == pathItem.Entity.Name);
                    string dtoName = GetDtoName(purposeDto, pathItem, withVia);
                    if (entitiesAlreadyGenerated.Add(dtoName))
                    {
                        GenerateDtoCore(dtoName, pathItem);
                    }

                    var otherWithVia = entitiesWithVia.Any(entity => entity.Name == pathItem.OtherEntity.Name);
                    string relationDtoName = GetRelationDtoName(purposeDto, pathItem, otherWithVia);
                    if (pathItemsAlreadyGenerated.Add(pathItem.ToString()))
                    {
                        if (index < purposeDtoProperty.PathItems.Count - 1)
                        {
                            HandleRelation(dtoName, relationDtoName.RemoveFirstOccurrence("I" + pathItem.OtherEntity.Name), pathItem);
                        }
                        else
                        {
                            HandleRelation(dtoName, "Dto", pathItem);
                        }
                    }
                }
            }
        }

        public HashSet<Entity> DetermineEntitiesWithVia(PurposeDto purposeDto)
        {
            var entitiesWithMultiplePaths = PurposeDtoPathHelper.FindEntitiesWithMultiplePathsAndIncludes(purposeDto);
            return entitiesWithMultiplePaths;
        }

        private void GenerateDtoCore(string dtoName, PurposeDtoPathItem pathItem)
        {
            this.entityCoreAddition.AddEntityToFrontend(
                pathItem.Entity,
                DTOsProjectGeneration.DomainFolder,
                TemplatePath,
                $"{dtoName.ToKebab()}.ts");
            
            this.iEntityDtoForPurposeClassRenamer.Rename(pathItem.Entity, dtoName, DTOsProjectGeneration.DomainFolder, $"{dtoName.ToKebab()}.ts");
        }

        private void HandleRelation(string dtoName, string dtoPostfix, PurposeDtoPathItem lastPathItem)
        {
            var isOneToOne = lastPathItem.Relation is Relation1To1;
            var isFrom = lastPathItem.Relation.TargetEntity == lastPathItem.Entity;

            if (isFrom)
            {
                if (isOneToOne)
                {
                    RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(lastPathItem.Relation, "I", dtoPostfix);

                    string fromImportStatementPath = $"@generated-app/dtos/{lastPathItem.Relation.SourceEntity.Module.NameKebab}" +
                                                     $"/{lastPathItem.Relation.SourceEntity.NamePluralKebab}" +
                                                     $"/dtos/i-{lastPathItem.Relation.SourceEntity.NameKebab}-" + dtoPostfix.ToKebab();

                    this.relationAddition.AddPropertyToDTO(relationSideFrom, DTOsProjectGeneration.DomainFolder, $"{dtoName.ToKebab()}.ts",
                        $"I{lastPathItem.Relation.SourceEntity.Name}{dtoPostfix}", fromImportStatementPath);
                }
                else
                {
                    RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(lastPathItem.Relation, "I", dtoPostfix + "[]");

                    string toImportStatementPath = $"@generated-app/dtos/{lastPathItem.Relation.SourceEntity.Module.NameKebab}" +
                                                   $"/{lastPathItem.Relation.SourceEntity.NamePluralKebab}" +
                                                   $"/dtos/i-{lastPathItem.Relation.SourceEntity.NameKebab}-" + dtoPostfix.ToKebab();

                    this.relationAddition.AddPropertyToDTO(relationSideFrom, DTOsProjectGeneration.DomainFolder, $"{dtoName.ToKebab()}.ts",
                        $"I{lastPathItem.Relation.SourceEntity.Name}{dtoPostfix}", toImportStatementPath);
                }
            }
            else
            {
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(lastPathItem.Relation, "I", dtoPostfix);
                
                string toImportStatementPath = $"@generated-app/dtos/{lastPathItem.Relation.TargetEntity.Module.NameKebab}" +
                                               $"/{lastPathItem.Relation.TargetEntity.NamePluralKebab}" +
                                               $"/dtos/i-{lastPathItem.Relation.TargetEntity.NameKebab}-" + dtoPostfix.ToKebab();
                
                this.relationAddition.AddPropertyToDTO(relationSideTo, DTOsProjectGeneration.DomainFolder, $"{dtoName.ToKebab()}.ts",
                    $"I{lastPathItem.Relation.TargetEntity.Name}{dtoPostfix}", toImportStatementPath);
            }
        }

        private string GetDtoName(PurposeDto purposeDto, PurposeDtoPathItem pathItem, bool withVia)
        {
            string viaSuffix = "";
            if (withVia)
            {
                viaSuffix = "Via" + FindViaPath(purposeDto, pathItem, true);
            }

            return $"I{pathItem.Entity}DtoFor{purposeDto.Purpose}{viaSuffix}";
        }

        private string GetRelationDtoName(PurposeDto purposeDto, PurposeDtoPathItem pathItem, bool withVia)
        {
            string viaSuffix = "";
            if (withVia)
            {
                viaSuffix = "Via" + FindViaPath(purposeDto, pathItem, false);
            }

            var otherEntityName = (pathItem.Relation.TargetEntity == pathItem.Entity ? pathItem.Relation.SourceEntity : pathItem.Relation.TargetEntity).Name;
            return $"I{otherEntityName}DtoFor{purposeDto.Purpose}{viaSuffix}";
        }

        private string FindViaPath(PurposeDto purposeDto, PurposeDtoPathItem targetPathItem, bool trimTargetEnd)
        {
            var delimiter = $"And";

            foreach (var purposeDtoProperty in purposeDto.Properties)
            {
                var path = string.Empty;

                foreach (var pathItem in purposeDtoProperty.PathItems)
                {
                    if (trimTargetEnd && pathItem == targetPathItem)
                    {
                        return path;
                    }

                    if (path.Length > 0)
                    {
                        path += delimiter;
                    }

                    path += $"{pathItem.PropertyName}";
                    
                    if (!trimTargetEnd && pathItem == targetPathItem)
                    {
                        return path;
                    }
                }
            }

            throw new KeyNotFoundException($"Der Pfad zur angegebenen Entität '{targetPathItem.PropertyName}' konnte nicht gefunden werden.");
        }

        public void AddInterface(GenerationOptions options, Interface interfaceItem)
        {
            foreach (var purposeDto in options.PurposeDtos)
            {
                string dtoName = GetDtoName(purposeDto, purposeDto.Properties.First().PathItems.First(), false);
                var entityInterfaceCompatibility = EntityInterfaceCompatibilityChecker.IsInterfaceCompatible(purposeDto.Entity, interfaceItem);
                if (entityInterfaceCompatibility != EntityInterfaceCompatibility.DtoData &&
                    entityInterfaceCompatibility != EntityInterfaceCompatibility.Dto &&
                    PurposeDtoInterfaceCompatibilityChecker.IsInterfaceCompatible(purposeDto, interfaceItem))
                {
                    this.interfaceExtender.AddInterface(
                        purposeDto.Entity,
                        interfaceItem.Name,
                        DTOsProjectGeneration.DomainFolder,
                        $"{dtoName.ToKebab()}.ts");
                }
            }
        }
    }
}