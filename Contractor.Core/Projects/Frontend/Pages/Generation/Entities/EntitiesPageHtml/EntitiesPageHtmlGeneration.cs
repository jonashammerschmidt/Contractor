using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab.page.html.template.txt");

        private static readonly string FileName = "entities-kebab.page.html";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntitiesPageHtmlPropertyAddition entitiesPageHtmlPropertyAddition;
        private readonly EntitiesPageHtmlToRelationAddition entitiesPageHtmlToRelationAddition;
        private readonly EntitiesPageHtmlFromOneToOneRelationAddition entitiesPageHtmlFromOneToOneRelationAddition;
        private readonly EntitiesPageHtmlToOneToOneRelationAddition entitiesPageHtmlToOneToOneRelationAddition;

        public EntityPageHtmlGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntitiesPageHtmlPropertyAddition entitiesPageHtmlPropertyAddition,
            EntitiesPageHtmlToRelationAddition entitiesPageHtmlToRelationAddition,
            EntitiesPageHtmlFromOneToOneRelationAddition entitiesPageHtmlFromOneToOneRelationAddition,
            EntitiesPageHtmlToOneToOneRelationAddition entitiesPageHtmlToOneToOneRelationAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entitiesPageHtmlPropertyAddition = entitiesPageHtmlPropertyAddition;
            this.entitiesPageHtmlToRelationAddition = entitiesPageHtmlToRelationAddition;
            this.entitiesPageHtmlFromOneToOneRelationAddition = entitiesPageHtmlFromOneToOneRelationAddition;
            this.entitiesPageHtmlToOneToOneRelationAddition = entitiesPageHtmlToOneToOneRelationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendPagesEntityCoreAddition.AddEntityCore(options, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.entitiesPageHtmlPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entitiesPageHtmlToRelationAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entitiesPageHtmlFromOneToOneRelationAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);

            this.entitiesPageHtmlToOneToOneRelationAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}