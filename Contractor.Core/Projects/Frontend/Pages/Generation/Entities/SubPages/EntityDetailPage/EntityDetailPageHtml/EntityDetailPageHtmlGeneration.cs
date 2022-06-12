using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityDetailPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.html.template.txt");

        private static readonly string FileName = "sub-pages\\detail\\entity-kebab-detail.page.html";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition;
        private readonly EntityDetailPageHtmlFromPropertyAddition entityDetailPageHtmlFromPropertyAddition;
        private readonly EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition;
        private readonly EntityDetailPageHtmlFromOneToOnePropertyAddition entityDetailPageHtmlFromOneToOnePropertyAddition;

        public EntityDetailPageHtmlGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition,
            EntityDetailPageHtmlFromPropertyAddition entityDetailPageHtmlFromPropertyAddition,
            EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition,
            EntityDetailPageHtmlFromOneToOnePropertyAddition entityDetailPageHtmlFromOneToOnePropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityDetailPageHtmlPropertyAddition = entityDetailPageHtmlPropertyAddition;
            this.entityDetailPageHtmlFromPropertyAddition = entityDetailPageHtmlFromPropertyAddition;
            this.entityDetailPageHtmlToPropertyAddition = entityDetailPageHtmlToPropertyAddition;
            this.entityDetailPageHtmlFromOneToOnePropertyAddition = entityDetailPageHtmlFromOneToOnePropertyAddition;
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
            this.entityDetailPageHtmlPropertyAddition.Edit(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            this.entityDetailPageHtmlFromPropertyAddition.Edit(relationSideFrom, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityDetailPageHtmlToPropertyAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            this.entityDetailPageHtmlFromOneToOnePropertyAddition.Edit(relationSideFrom, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityDetailPageHtmlToPropertyAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}