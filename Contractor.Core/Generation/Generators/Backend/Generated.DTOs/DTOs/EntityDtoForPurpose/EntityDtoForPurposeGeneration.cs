using System.Collections.Generic;
using Contractor.Core.BaseClasses;
using System.IO;
using System.Linq;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntityDtoForPurposeGeneration : IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoForPurposeTemplate.txt");

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDtoForPurposeClassRenamer entityDtoForPurposeClassRenamer;
        private readonly EntityDtoForPurposeFromMethodsAddition entityDtoForPurposeFromMethodsAddition;
        private readonly EntityDtoForPurposeFromOneToOneMethodsAddition entityDtoForPurposeFromOneToOneMethodsAddition;
        private readonly EntityDtoForPurposeToMethodsAddition entityDtoForPurposeToMethodsAddition;
        private readonly EntityDtoForPurposeIncludeInserter entityDtoForPurposeIncludeInserter;
        private readonly InterfaceExtender interfaceExtender;

        public EntityDtoForPurposeGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoRelationAddition relationAddition,
            EntityDtoForPurposeClassRenamer entityDtoForPurposeClassRenamer,
            EntityDtoForPurposeFromMethodsAddition entityDtoForPurposeFromMethodsAddition,
            EntityDtoForPurposeFromOneToOneMethodsAddition entityDtoForPurposeFromOneToOneMethodsAddition,
            EntityDtoForPurposeToMethodsAddition entityDtoForPurposeToMethodsAddition,
            EntityDtoForPurposeIncludeInserter entityDtoForPurposeIncludeInserter,
            InterfaceExtender interfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.relationAddition = relationAddition;
            this.entityDtoForPurposeClassRenamer = entityDtoForPurposeClassRenamer;
            this.entityDtoForPurposeFromMethodsAddition = entityDtoForPurposeFromMethodsAddition;
            this.entityDtoForPurposeFromOneToOneMethodsAddition = entityDtoForPurposeFromOneToOneMethodsAddition;
            this.entityDtoForPurposeToMethodsAddition = entityDtoForPurposeToMethodsAddition;
            this.entityDtoForPurposeIncludeInserter = entityDtoForPurposeIncludeInserter;
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

                        if (isRoot)
                        {
                            this.entityDtoForPurposeIncludeInserter.Insert(purposeDto, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs");
                        }
                    }

                    var otherWithVia = entitiesWithVia.Any(entity => entity.Name == pathItem.OtherEntity.Name);
                    string relationDtoName = GetRelationDtoName(purposeDto, pathItem, otherWithVia);
                    if (pathItemsAlreadyGenerated.Add(pathItem.ToString()))
                    {
                        if (index < purposeDtoProperty.PathItems.Count - 1)
                        {
                            HandleRelation(dtoName, relationDtoName.RemoveFirstOccurrence(pathItem.OtherEntity.Name), pathItem);
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
            this.entityCoreAddition.AddEntityToBackendGenerated(
                pathItem.Entity,
                GeneratedDTOsProjectGeneration.DomainFolder,
                TemplatePath,
                $"{dtoName}.cs");
            
            this.entityDtoForPurposeClassRenamer.Rename(pathItem.Entity, dtoName, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs");
        }

        private void HandleRelation(string dtoName, string dtoPostfix, PurposeDtoPathItem lastPathItem)
        {
            var isOneToOne = lastPathItem.Relation is Relation1To1;
            var isFrom = lastPathItem.Relation.TargetEntity == lastPathItem.Entity;

            if (isFrom)
            {
                if (isOneToOne)
                {
                    RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(lastPathItem.Relation, "", dtoPostfix);

                    this.relationAddition.AddRelationToDTOForBackendGenerated(
                        relationSideFrom,
                        GeneratedDTOsProjectGeneration.DomainFolder,
                        $"{dtoName}.cs",
                        $"{relationSideFrom.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

                    this.entityDtoForPurposeFromOneToOneMethodsAddition.AddRelationSideToBackendGeneratedFile(
                        relationSideFrom,
                        relationSideFrom.OtherEntity.Name + dtoPostfix,
                        GeneratedDTOsProjectGeneration.DomainFolder,
                        $"{dtoName}.cs");
                }
                else
                {
                    RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(lastPathItem.Relation, "IEnumerable<", dtoPostfix + ">");

                    this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideFrom, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs",
                        $"{relationSideFrom.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

                    this.entityDtoForPurposeFromMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, relationSideFrom.OtherEntity.Name + dtoPostfix, GeneratedDTOsProjectGeneration.DomainFolder,
                        $"{dtoName}.cs");
                }
            }
            else
            {
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(lastPathItem.Relation, "", dtoPostfix);

                this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs",
                    $"{relationSideTo.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

                this.entityDtoForPurposeToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder,
                    $"{dtoName}.cs");
            }
        }

        private string GetDtoName(PurposeDto purposeDto, PurposeDtoPathItem pathItem, bool withVia)
        {
            string viaSuffix = "";
            if (withVia)
            {
                viaSuffix = "Via" + FindViaPath(purposeDto, pathItem, true);
            }

            return $"{pathItem.Entity}DtoFor{purposeDto.Purpose}{viaSuffix}";
        }

        private string GetRelationDtoName(PurposeDto purposeDto, PurposeDtoPathItem pathItem, bool withVia)
        {
            string viaSuffix = "";
            if (withVia)
            {
                viaSuffix = "Via" + FindViaPath(purposeDto, pathItem, false);
            }

            var otherEntityName = (pathItem.Relation.TargetEntity == pathItem.Entity ? pathItem.Relation.SourceEntity : pathItem.Relation.TargetEntity).Name;
            return $"{otherEntityName}DtoFor{purposeDto.Purpose}{viaSuffix}";
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
                    this.interfaceExtender.AddInterfaceToClass(
                        purposeDto.Entity,
                        interfaceItem.Name,
                        GeneratedDTOsProjectGeneration.DomainFolder,
                        $"{dtoName}.cs");
                }
            }
        }
    }
}
