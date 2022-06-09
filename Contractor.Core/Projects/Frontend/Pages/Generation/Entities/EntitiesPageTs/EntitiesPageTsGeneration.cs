using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntitiesPageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab.page.ts.template.txt");

        private static readonly string FileName = "entities-kebab.page.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntitiesPageTsPropertyAddition entitiesPageTsPropertyAddition;
        private readonly EntitiesPageTsToPropertyAddition entitiesPageTsToPropertyAddition;
        private readonly EntitiesPageTsFromOneToOnePropertyAddition entitiesPageTsFromOneToOnePropertyAddition;
        private readonly EntitiesPageTsToOneToOnePropertyAddition entitiesPageTsToOneToOnePropertyAddition;

        public EntitiesPageTsGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntitiesPageTsPropertyAddition entitiesPageTsPropertyAddition,
            EntitiesPageTsToPropertyAddition entitiesPageTsToPropertyAddition,
            EntitiesPageTsFromOneToOnePropertyAddition entitiesPageTsFromOneToOnePropertyAddition,
            EntitiesPageTsToOneToOnePropertyAddition entitiesPageTsToOneToOnePropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entitiesPageTsPropertyAddition = entitiesPageTsPropertyAddition;
            this.entitiesPageTsToPropertyAddition = entitiesPageTsToPropertyAddition;
            this.entitiesPageTsFromOneToOnePropertyAddition = entitiesPageTsFromOneToOnePropertyAddition;
            this.entitiesPageTsToOneToOnePropertyAddition = entitiesPageTsToOneToOnePropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntity(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.entitiesPageTsPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entitiesPageTsToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            this.entitiesPageTsFromOneToOnePropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);

            // To
            this.entitiesPageTsToOneToOnePropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}