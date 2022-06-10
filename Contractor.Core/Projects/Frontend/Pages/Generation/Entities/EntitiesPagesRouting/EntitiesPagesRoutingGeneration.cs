using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntitiesPagesRoutingGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab-pages.routing.template.txt");

        private static readonly string FileName = "entities-kebab-pages.routing.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;

        public EntitiesPagesRoutingGeneration(
            FrontendEntityAddition frontendEntityCoreAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntity(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
        }
    }
}