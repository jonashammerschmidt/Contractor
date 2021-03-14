using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab.page.ts.template.txt");

        private static readonly string FileName = "entities-kebab.page.ts";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntitiesPageTsPropertyAddition entitiesPageTsPropertyAddition;
        
        public EntitiesPageTsGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntitiesPageTsPropertyAddition entitiesPageTsPropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entitiesPageTsPropertyAddition = entitiesPageTsPropertyAddition;
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
            this.entitiesPageTsPropertyAddition.Add(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}