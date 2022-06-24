using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab.page.html.template.txt");

        private static readonly string FileName = "entities-kebab.page.html";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntitiesPageHtmlPropertyAddition entitiesPageHtmlPropertyAddition;
        private readonly EntitiesPageHtmlToRelationAddition entitiesPageHtmlToRelationAddition;
        private readonly EntitiesPageHtmlFromOneToOneRelationAddition entitiesPageHtmlFromOneToOneRelationAddition;
        private readonly EntitiesPageHtmlToOneToOneRelationAddition entitiesPageHtmlToOneToOneRelationAddition;

        public EntityPageHtmlGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntitiesPageHtmlPropertyAddition entitiesPageHtmlPropertyAddition,
            EntitiesPageHtmlToRelationAddition entitiesPageHtmlToRelationAddition,
            EntitiesPageHtmlFromOneToOneRelationAddition entitiesPageHtmlFromOneToOneRelationAddition,
            EntitiesPageHtmlToOneToOneRelationAddition entitiesPageHtmlToOneToOneRelationAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entitiesPageHtmlPropertyAddition = entitiesPageHtmlPropertyAddition;
            this.entitiesPageHtmlToRelationAddition = entitiesPageHtmlToRelationAddition;
            this.entitiesPageHtmlFromOneToOneRelationAddition = entitiesPageHtmlFromOneToOneRelationAddition;
            this.entitiesPageHtmlToOneToOneRelationAddition = entitiesPageHtmlToOneToOneRelationAddition;
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
            this.entitiesPageHtmlPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entitiesPageHtmlToRelationAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            this.entitiesPageHtmlFromOneToOneRelationAddition.AddRelationSideToFrontendFile(relationSideFrom, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entitiesPageHtmlToOneToOneRelationAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}