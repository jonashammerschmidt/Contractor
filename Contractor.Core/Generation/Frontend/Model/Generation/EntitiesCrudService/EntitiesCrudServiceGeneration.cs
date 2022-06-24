using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class EntitiesCrudServiceGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "entities-kebab-crud.service.template.txt");

        private static readonly string FileName = "entities-kebab-crud.service.ts";

        private readonly EntityCoreAddition frontendModelEntityCoreAddition;

        public EntitiesCrudServiceGeneration(
            EntityCoreAddition frontendModelEntityCoreAddition)
        {
            this.frontendModelEntityCoreAddition = frontendModelEntityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendModelEntityCoreAddition.AddEntityToFrontend(entity, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }
    }
}