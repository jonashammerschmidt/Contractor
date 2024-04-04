using System.Collections.Generic;
using Contractor.Core.BaseClasses;
using System.IO;
using System.Linq;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntityDtoForPurposeGeneration
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

        public EntityDtoForPurposeGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoRelationAddition relationAddition,
            EntityDtoForPurposeClassRenamer entityDtoForPurposeClassRenamer,
            EntityDtoForPurposeFromMethodsAddition entityDtoForPurposeFromMethodsAddition,
            EntityDtoForPurposeFromOneToOneMethodsAddition entityDtoForPurposeFromOneToOneMethodsAddition,
            EntityDtoForPurposeToMethodsAddition entityDtoForPurposeToMethodsAddition,
            EntityDtoForPurposeIncludeInserter entityDtoForPurposeIncludeInserter)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.relationAddition = relationAddition;
            this.entityDtoForPurposeClassRenamer = entityDtoForPurposeClassRenamer;
            this.entityDtoForPurposeFromMethodsAddition = entityDtoForPurposeFromMethodsAddition;
            this.entityDtoForPurposeFromOneToOneMethodsAddition = entityDtoForPurposeFromOneToOneMethodsAddition;
            this.entityDtoForPurposeToMethodsAddition = entityDtoForPurposeToMethodsAddition;
            this.entityDtoForPurposeIncludeInserter = entityDtoForPurposeIncludeInserter;
        }

        public void Generate(CustomDto customDto)
        {
            var entitiesWithVia = DetermineEntitiesWithVia(customDto);
            var entitiesAlreadyGenerated = new HashSet<string>();
            var pathItemsAlreadyGenerated = new HashSet<string>();

            foreach (var customDtoProperty in customDto.Properties)
            {
                for (var index = 0; index < customDtoProperty.PathItems.Count; index++)
                {
                    var pathItem = customDtoProperty.PathItems[index];

                    bool isRoot = index == 0;
                    var withVia = !isRoot && entitiesWithVia.Any(entity => entity.Name == pathItem.Entity.Name);
                    string dtoName = GetDtoName(customDto, pathItem, withVia);
                    if (entitiesAlreadyGenerated.Add(dtoName))
                    {
                        GenerateDtoCore(dtoName, pathItem);

                        if (isRoot)
                        {
                            this.entityDtoForPurposeIncludeInserter.Insert(customDto, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs");
                        }
                    }

                    var otherWithVia = entitiesWithVia.Any(entity => entity.Name == pathItem.OtherEntity.Name);
                    string relationDtoName = GetRelationDtoName(customDto, pathItem, otherWithVia);
                    if (pathItemsAlreadyGenerated.Add(pathItem.ToString()))
                    {
                        if (index < customDtoProperty.PathItems.Count - 1)
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

        public HashSet<Entity> DetermineEntitiesWithVia(CustomDto customDto)
        {
            var entitiesWithMultiplePaths = CustomDtoPathHelper.FindEntitiesWithMultiplePathsAndIncludes(customDto);
            return entitiesWithMultiplePaths;
        }

        private void GenerateDtoCore(string dtoName, CustomDtoPathItem pathItem)
        {
            this.entityCoreAddition.AddEntityToBackendGenerated(
                pathItem.Entity,
                GeneratedDTOsProjectGeneration.DomainFolder,
                TemplatePath,
                $"{dtoName}.cs");
            
            this.entityDtoForPurposeClassRenamer.Rename(pathItem.Entity, dtoName, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs");
        }

        private void HandleRelation(string dtoName, string dtoPostfix, CustomDtoPathItem lastPathItem)
        {
            var isOneToOne = lastPathItem.Relation is Relation1To1;
            var isFrom = lastPathItem.Relation.EntityFrom == lastPathItem.Entity;

            if (isFrom)
            {
                if (isOneToOne)
                {
                    RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(lastPathItem.Relation, "", dtoPostfix);

                    this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideFrom, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs",
                        $"{relationSideFrom.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

                    this.entityDtoForPurposeFromOneToOneMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedDTOsProjectGeneration.DomainFolder,
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

        private string GetDtoName(CustomDto customDto, CustomDtoPathItem pathItem, bool withVia)
        {
            string viaSuffix = "";
            if (withVia)
            {
                viaSuffix = "Via" + FindViaPath(customDto, pathItem, true);
            }

            return $"{pathItem.Entity}DtoFor{customDto.Purpose}{viaSuffix}";
        }

        private string GetRelationDtoName(CustomDto customDto, CustomDtoPathItem pathItem, bool withVia)
        {
            string viaSuffix = "";
            if (withVia)
            {
                viaSuffix = "Via" + FindViaPath(customDto, pathItem, false);
            }

            var otherEntityName = (pathItem.Relation.EntityFrom == pathItem.Entity ? pathItem.Relation.EntityTo : pathItem.Relation.EntityFrom).Name;
            return $"{otherEntityName}DtoFor{customDto.Purpose}{viaSuffix}";
        }

        private string FindViaPath(CustomDto customDto, CustomDtoPathItem targetPathItem, bool trimTargetEnd)
        {
            var delimiter = $"And";

            foreach (var customDtoProperty in customDto.Properties)
            {
                var path = string.Empty;

                foreach (var pathItem in customDtoProperty.PathItems)
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
    }
}