using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_DTOS })]
    internal class EntityDtoExpandedGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoExpandedTemplate.txt");

        private static readonly string FileName = "EntityDtoExpanded.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDtoExpandedMethodsAddition entityDtoExpandedMethodsAddition;
        private readonly EntityDtoExpandedToMethodsAddition entityDtoExpandedToMethodsAddition;
        private readonly EntityDtoExpandedFromOneToOneMethodsAddition entityDtoExpandedFromOneToOneMethodsAddition;

        public EntityDtoExpandedGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition,
            EntityDtoExpandedMethodsAddition entityDtoExpandedMethodsAddition,
            EntityDtoExpandedToMethodsAddition entityDtoExpandedToMethodsAddition,
            EntityDtoExpandedFromOneToOneMethodsAddition entityDtoExpandedFromOneToOneMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.entityDtoExpandedMethodsAddition = entityDtoExpandedMethodsAddition;
            this.entityDtoExpandedToMethodsAddition = entityDtoExpandedToMethodsAddition;
            this.entityDtoExpandedFromOneToOneMethodsAddition = entityDtoExpandedFromOneToOneMethodsAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedDTOsProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
            this.entityDtoExpandedMethodsAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "Dto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Generated.DTOs.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.entityDtoExpandedToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "Dto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideFrom, GeneratedDTOsProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Generated.DTOs.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.entityDtoExpandedFromOneToOneMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "Dto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Generated.DTOs.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.entityDtoExpandedToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }
    }
}