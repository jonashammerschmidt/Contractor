using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_DTOS })]
    public class EntityDtoGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoTemplate.txt");

        private static readonly string FileName = "EntityDto.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly ApiDtoPropertyAddition apiPropertyAddition;
        private readonly EntityDtoMethodsAddition entityDtoMethodsAddition;

        public EntityDtoGeneration(
            EntityCoreAddition entityCoreAddition,
            ApiDtoPropertyAddition apiPropertyAddition,
            EntityDtoMethodsAddition entityDtoMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.apiPropertyAddition = apiPropertyAddition;
            this.entityDtoMethodsAddition = entityDtoMethodsAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedDTOsProjectGeneration.DomainFolder, templatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            if (property.IsHidden)
            {
                return;
            }

            this.entityDtoMethodsAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.entityDtoMethodsAddition.AddPropertyToBackendGeneratedFile(relationSide, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            this.Add1ToNRelationSideTo(new Relation1ToN(relation));
        }
    }
}