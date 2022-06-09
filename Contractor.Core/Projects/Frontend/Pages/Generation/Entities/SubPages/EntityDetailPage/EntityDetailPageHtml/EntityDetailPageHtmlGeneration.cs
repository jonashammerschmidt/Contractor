using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityDetailPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.html.template.txt");

        private static readonly string FileName = "sub-pages\\detail\\entity-kebab-detail.page.html";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition;
        private readonly EntityDetailPageHtmlFromPropertyAddition entityDetailPageHtmlFromPropertyAddition;
        private readonly EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition;
        private readonly EntityDetailPageHtmlFromOneToOnePropertyAddition entityDetailPageHtmlFromOneToOnePropertyAddition;

        public EntityDetailPageHtmlGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition,
            EntityDetailPageHtmlFromPropertyAddition entityDetailPageHtmlFromPropertyAddition,
            EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition,
            EntityDetailPageHtmlFromOneToOnePropertyAddition entityDetailPageHtmlFromOneToOnePropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityDetailPageHtmlPropertyAddition = entityDetailPageHtmlPropertyAddition;
            this.entityDetailPageHtmlFromPropertyAddition = entityDetailPageHtmlFromPropertyAddition;
            this.entityDetailPageHtmlToPropertyAddition = entityDetailPageHtmlToPropertyAddition;
            this.entityDetailPageHtmlFromOneToOnePropertyAddition = entityDetailPageHtmlFromOneToOnePropertyAddition;
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
            this.entityDetailPageHtmlPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entityDetailPageHtmlFromPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);

            this.entityDetailPageHtmlToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entityDetailPageHtmlFromOneToOnePropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);

            this.entityDetailPageHtmlToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}