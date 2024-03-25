using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    public class DomainModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "domain-kebab-pages.module.template.txt");

        private static readonly string FileName = "domain-kebab-pages.module.ts";

        private readonly ModuleCoreAddition moduleCoreAddition;

        public DomainModuleGeneration(
            ModuleCoreAddition moduleCoreAddition)
        {
            this.moduleCoreAddition = moduleCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
            this.moduleCoreAddition.AddModuleToFrontend(module, PagesProjectGeneration.PagesFolder, TemplatePath, FileName);
        }

        protected override void AddEntity(Entity entity)
        {
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