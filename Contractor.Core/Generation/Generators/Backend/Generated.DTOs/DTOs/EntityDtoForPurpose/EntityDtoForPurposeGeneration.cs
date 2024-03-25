using System.Collections.Generic;
using Contractor.Core.BaseClasses;
using System.IO;
using System.Linq;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class EntityDtoForPurposeGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoForPurposeTemplate.txt");

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDtoForPurposeClassRenamer entityDtoForPurposeClassRenamer;
        private readonly EntityDtoForPurposeFromMethodsAddition entityDtoForPurposeFromMethodsAddition;
        private readonly EntityDtoForPurposeFromOneToOneMethodsAddition entityDtoForPurposeFromOneToOneMethodsAddition;
        private readonly EntityDtoForPurposeToMethodsAddition entityDtoForPurposeToMethodsAddition;

        public EntityDtoForPurposeGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoRelationAddition relationAddition,
            EntityDtoForPurposeClassRenamer entityDtoForPurposeClassRenamer,
            EntityDtoForPurposeFromMethodsAddition entityDtoForPurposeFromMethodsAddition,
            EntityDtoForPurposeFromOneToOneMethodsAddition entityDtoForPurposeFromOneToOneMethodsAddition,
            EntityDtoForPurposeToMethodsAddition entityDtoForPurposeToMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.relationAddition = relationAddition;
            this.entityDtoForPurposeClassRenamer = entityDtoForPurposeClassRenamer;
            this.entityDtoForPurposeFromMethodsAddition = entityDtoForPurposeFromMethodsAddition;
            this.entityDtoForPurposeFromOneToOneMethodsAddition = entityDtoForPurposeFromOneToOneMethodsAddition;
            this.entityDtoForPurposeToMethodsAddition = entityDtoForPurposeToMethodsAddition;
        }

        public void Generate(CustomDto customDto)
        {
            var entitiesWithVia = DetermineEntitiesWithVia(customDto);
            var entitiesAlreadyGenerated = new HashSet<Entity>();
            var pathItemsAlreadyGenerated = new HashSet<string>();

            foreach (var customDtoProperty in customDto.Properties)
            {
                for (var index = 0; index < customDtoProperty.PathItems.Count; index++)
                {
                    var pathItem = customDtoProperty.PathItems[index];
                    string dtoName = GetDtoName(customDto, pathItem, entitiesWithVia.Contains(pathItem.Entity));
                    string relationDtoName = GetRelationDtoName(customDto, pathItem, entitiesWithVia.Contains(pathItem.Entity));
                    if (entitiesAlreadyGenerated.Add(pathItem.Entity))
                    {
                        GenerateDtoCore(dtoName, pathItem);
                    }

                    if (pathItemsAlreadyGenerated.Add(pathItem.ToString()))
                    {
                        if (index < customDtoProperty.PathItems.Count - 1)
                        {
                            HandleRelation(dtoName, relationDtoName, pathItem);
                        }
                        else
                        {
                            HandleRelationForLastEntity(dtoName, pathItem);
                        }
                    }
                }
            }
        }

        private HashSet<Entity> DetermineEntitiesWithVia(CustomDto customDto)
        {
            var entitiesToGenerate = new HashSet<Entity>();
            entitiesToGenerate.Add(customDto.Entity);
            var entitiesWithVia = new HashSet<Entity>();

            foreach (var customDtoProperty in customDto.Properties)
            {
                foreach (var pathItem in customDtoProperty.PathItems.Skip(1))
                {
                    var entity = pathItem.Entity;
                    if (!entitiesToGenerate.Add(entity))
                    {
                        entitiesWithVia.Add(entity);
                    }
                }
            }

            return entitiesWithVia;
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

        private void HandleRelation(string dtoName, string relationDtoName, CustomDtoPathItem pathItem)
        {
            var isOneToOne = pathItem.Relation is Relation1To1;
            var isFrom = pathItem.Relation.EntityFrom == pathItem.Entity;

            if (isFrom)
            {
                if (isOneToOne)
                {
                }
                else
                {
                }
            }
            else
            {
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(pathItem.Relation, "", relationDtoName.Replace(pathItem.Relation.EntityFrom.Name, ""));
                relationSideTo.IsNew = true;

                this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, $"{dtoName}.cs",
                    $"{relationSideTo.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

                this.entityDtoForPurposeToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder,
                    $"{dtoName}.cs");
            }
        }

        private void HandleRelationForLastEntity(string dtoName, CustomDtoPathItem lastPathItem)
        {
            var isOneToOne = lastPathItem.Relation is Relation1To1;
            var isFrom = lastPathItem.Relation.EntityFrom == lastPathItem.Entity;

            if (isFrom)
            {
                if (isOneToOne)
                {
                }
                else
                {
                }
            }
            else
            {
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(lastPathItem.Relation, "", "DtoExpanded");
                relationSideTo.IsNew = true;

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