using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_DTOS })]
    internal class EntityDetailDefaultDtoGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDetailDefaultDtoTemplate.txt");

        private static readonly string FileName = "EntityDetailDefaultDto.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDetailDefaultDtoMethodsAddition entityDetailDefaultDtoMethodsAddition;
        private readonly EntityDetailDefaultDtoToMethodsAddition entityDetailDefaultDtoToMethodsAddition;
        private readonly EntityDetailDefaultDtoFromOneToOneMethodsAddition entityDetailDefaultDtoFromOneToOneMethodsAddition;

        public EntityDetailDefaultDtoGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition,
            EntityDetailDefaultDtoMethodsAddition entityDetailDefaultDtoMethodsAddition,
            EntityDetailDefaultDtoToMethodsAddition entityDetailDefaultDtoToMethodsAddition,
            EntityDetailDefaultDtoFromOneToOneMethodsAddition entityDetailDefaultDtoFromOneToOneMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.entityDetailDefaultDtoMethodsAddition = entityDetailDefaultDtoMethodsAddition;
            this.entityDetailDefaultDtoToMethodsAddition = entityDetailDefaultDtoToMethodsAddition;
            this.entityDetailDefaultDtoFromOneToOneMethodsAddition = entityDetailDefaultDtoFromOneToOneMethodsAddition;
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
            this.entityDetailDefaultDtoMethodsAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "DetailDefaultDto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Generated.DTOs.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.entityDetailDefaultDtoToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "DetailDefaultDto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideFrom, GeneratedDTOsProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Generated.DTOs.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.entityDetailDefaultDtoFromOneToOneMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "DetailDefaultDto");

            this.relationAddition.AddRelationToDTOForBackendGenerated(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Generated.DTOs.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.entityDetailDefaultDtoToMethodsAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }
    }
}