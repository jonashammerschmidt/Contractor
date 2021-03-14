using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class DomainRoutingGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "domain-kebab-pages.routing.template.txt");

        private static readonly string FileName = "domain-kebab-pages.routing.ts";

        private readonly FrontendPagesDomainCoreAddition frontendPagesDomainCoreAddition;
        private readonly DomainRoutingEntityAddition domainRoutingEntityAddition;

        public DomainRoutingGeneration(
            FrontendPagesDomainCoreAddition frontendPagesDomainCoreAddition,
            DomainRoutingEntityAddition domainRoutingEntityAddition)
        {
            this.frontendPagesDomainCoreAddition = frontendPagesDomainCoreAddition;
            this.domainRoutingEntityAddition = domainRoutingEntityAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
            this.frontendPagesDomainCoreAddition.AddEntityCore(options, PagesProjectGeneration.PagesFolder, TemplatePath, FileName);
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.domainRoutingEntityAddition.Add(options, PagesProjectGeneration.PagesFolder, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}