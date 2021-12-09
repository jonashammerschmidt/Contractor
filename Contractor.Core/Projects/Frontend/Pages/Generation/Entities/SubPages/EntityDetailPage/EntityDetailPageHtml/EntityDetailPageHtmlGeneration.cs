using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.html.template.txt");

        private static readonly string FileName = "sub-pages\\detail\\entity-kebab-detail.page.html";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition;
        private readonly EntityDetailPageHtmlFromPropertyAddition entityDetailPageHtmlFromPropertyAddition;
        private readonly EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition;
        private readonly EntityDetailPageHtmlFromOneToOnePropertyAddition entityDetailPageHtmlFromOneToOnePropertyAddition;

        public EntityDetailPageHtmlGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition,
            EntityDetailPageHtmlFromPropertyAddition entityDetailPageHtmlFromPropertyAddition,
            EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition,
            EntityDetailPageHtmlFromOneToOnePropertyAddition entityDetailPageHtmlFromOneToOnePropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entityDetailPageHtmlPropertyAddition = entityDetailPageHtmlPropertyAddition;
            this.entityDetailPageHtmlFromPropertyAddition = entityDetailPageHtmlFromPropertyAddition;
            this.entityDetailPageHtmlToPropertyAddition = entityDetailPageHtmlToPropertyAddition;
            this.entityDetailPageHtmlFromOneToOnePropertyAddition = entityDetailPageHtmlFromOneToOnePropertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendPagesEntityCoreAddition.AddEntityCore(options, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
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