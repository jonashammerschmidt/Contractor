using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntitiesPageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab.page.ts.template.txt");

        private static readonly string FileName = "entities-kebab.page.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntitiesPageTsPropertyAddition entitiesPageTsPropertyAddition;
        private readonly EntitiesPageTsToPropertyAddition entitiesPageTsToPropertyAddition;
        private readonly EntitiesPageTsFromOneToOnePropertyAddition entitiesPageTsFromOneToOnePropertyAddition;
        private readonly EntitiesPageTsToOneToOnePropertyAddition entitiesPageTsToOneToOnePropertyAddition;

        public EntitiesPageTsGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntitiesPageTsPropertyAddition entitiesPageTsPropertyAddition,
            EntitiesPageTsToPropertyAddition entitiesPageTsToPropertyAddition,
            EntitiesPageTsFromOneToOnePropertyAddition entitiesPageTsFromOneToOnePropertyAddition,
            EntitiesPageTsToOneToOnePropertyAddition entitiesPageTsToOneToOnePropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entitiesPageTsPropertyAddition = entitiesPageTsPropertyAddition;
            this.entitiesPageTsToPropertyAddition = entitiesPageTsToPropertyAddition;
            this.entitiesPageTsFromOneToOnePropertyAddition = entitiesPageTsFromOneToOnePropertyAddition;
            this.entitiesPageTsToOneToOnePropertyAddition = entitiesPageTsToOneToOnePropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntity(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.entitiesPageTsPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entitiesPageTsToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            this.entitiesPageTsFromOneToOnePropertyAddition.AddRelationSideToFrontendFile(relationSideFrom, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entitiesPageTsToOneToOnePropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}