using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-create.page.html.template.txt");

        private static readonly string FileName = "entity-kebab-create\\entity-kebab-create.page.html";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntityCreatePageHtmlPropertyAddition entityCreatePageHtmlPropertyAddition;

        public EntityCreatePageHtmlGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntityCreatePageHtmlPropertyAddition entityCreatePageHtmlPropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entityCreatePageHtmlPropertyAddition = entityCreatePageHtmlPropertyAddition;
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
            this.entityCreatePageHtmlPropertyAddition.Add(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}