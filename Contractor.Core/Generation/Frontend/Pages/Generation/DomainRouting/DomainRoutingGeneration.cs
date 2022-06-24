using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class DomainRoutingGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "domain-kebab-pages.routing.template.txt");

        private static readonly string FileName = "domain-kebab-pages.routing.ts";

        private readonly ModuleCoreAddition moduleCoreAddition;
        private readonly DomainRoutingEntityAddition domainRoutingEntityAddition;

        public DomainRoutingGeneration(
            ModuleCoreAddition moduleCoreAddition,
            DomainRoutingEntityAddition domainRoutingEntityAddition)
        {
            this.moduleCoreAddition = moduleCoreAddition;
            this.domainRoutingEntityAddition = domainRoutingEntityAddition;
        }

        protected override void AddModuleActions(Module module)
        {
            this.moduleCoreAddition.AddModuleToFrontend(module, PagesProjectGeneration.PagesFolder, TemplatePath, FileName);
        }

        protected override void AddEntity(Entity entity)
        {
            this.domainRoutingEntityAddition.Add(entity, PagesProjectGeneration.PagesFolder, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }
    }
}