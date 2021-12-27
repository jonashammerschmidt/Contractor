using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.html.template.txt");

        private static readonly string FileName = "dialogs\\entity-kebab-update\\entity-kebab-update.dialog.html";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntityUpdatePageHtmlPropertyAddition entityUpdatePageHtmlPropertyAddition;
        private readonly EntityUpdatePageHtmlToPropertyAddition entityUpdatePageHtmlToPropertyAddition;

        public EntityUpdatePageHtmlGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntityUpdatePageHtmlPropertyAddition entityUpdatePageHtmlPropertyAddition,
            EntityUpdatePageHtmlToPropertyAddition entityUpdatePageHtmlToPropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entityUpdatePageHtmlPropertyAddition = entityUpdatePageHtmlPropertyAddition;
            this.entityUpdatePageHtmlToPropertyAddition = entityUpdatePageHtmlToPropertyAddition;
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
            this.entityUpdatePageHtmlPropertyAddition.Add(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entityUpdatePageHtmlToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}