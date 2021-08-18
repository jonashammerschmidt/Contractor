using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class DomainModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "domain-kebab-pages.module.template.txt");

        private static readonly string FileName = "domain-kebab-pages.module.ts";

        private readonly FrontendPagesDomainCoreAddition frontendPagesDomainCoreAddition;

        public DomainModuleGeneration(
            FrontendPagesDomainCoreAddition frontendPagesDomainCoreAddition)
        {
            this.frontendPagesDomainCoreAddition = frontendPagesDomainCoreAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
            this.frontendPagesDomainCoreAddition.AddEntityCore(options, PagesProjectGeneration.PagesFolder, TemplatePath, FileName);
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}