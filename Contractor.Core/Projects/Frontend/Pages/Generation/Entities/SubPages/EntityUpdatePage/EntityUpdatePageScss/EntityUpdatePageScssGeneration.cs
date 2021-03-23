using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageScssGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.scss.template.txt");

        private static readonly string FileName = "sub-pages\\entity-kebab-update\\entity-kebab-update.page.scss";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;

        public EntityUpdatePageScssGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
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
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}