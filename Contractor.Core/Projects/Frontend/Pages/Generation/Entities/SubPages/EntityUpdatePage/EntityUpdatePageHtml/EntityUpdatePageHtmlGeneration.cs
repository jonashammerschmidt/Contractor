using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.html.template.txt");

        private static readonly string FileName = "entity-kebab-update\\entity-kebab-update.page.html";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntityUpdatePageHtmlPropertyAddition entityUpdatePageHtmlPropertyAddition;

        public EntityUpdatePageHtmlGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntityUpdatePageHtmlPropertyAddition entityUpdatePageHtmlPropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entityUpdatePageHtmlPropertyAddition = entityUpdatePageHtmlPropertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            // TODO: Fix default by making it a function
            this.frontendPagesEntityCoreAddition.AddEntityCore(options, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.entityUpdatePageHtmlPropertyAddition.Add(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}