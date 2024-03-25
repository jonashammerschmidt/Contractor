using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_DTOS })]
    public class EntityDtoExpandedGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoExpandedTemplate.txt");

        private static readonly string FileName = "EntityDtoExpanded.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDtoExpandedToMethodsAddition entityDtoExpandedToMethodsAddition;
        private readonly EntityDtoExpandedFromOneToOneMethodsAddition entityDtoExpandedFromOneToOneMethodsAddition;

        public EntityDtoExpandedGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition,
            EntityDtoExpandedToMethodsAddition entityDtoExpandedToMethodsAddition,
            EntityDtoExpandedFromOneToOneMethodsAddition entityDtoExpandedFromOneToOneMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.entityDtoExpandedToMethodsAddition = entityDtoExpandedToMethodsAddition;
            this.entityDtoExpandedFromOneToOneMethodsAddition = entityDtoExpandedFromOneToOneMethodsAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedDTOsProjectGeneration.DomainFolder, TemplatePath, FileName);

            if (entity.HasScope)
            {      
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(entity.ScopeEntity, entity, "", "Dto");

                this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, FileName,
                    $"{relationSideTo.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

                this.entityDtoExpandedToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
            }
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "Dto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.entityDtoExpandedToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "Dto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideFrom, GeneratedDTOsProjectGeneration.DomainFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.entityDtoExpandedFromOneToOneMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "Dto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.entityDtoExpandedToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }
    }
}