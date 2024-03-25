using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    public class EntityDetailPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.html.template.txt");

        private static readonly string FileName = Path.Combine("sub-pages", "detail", "entity-kebab-detail.page.html");

        private readonly EntityCoreAddition frontendEntityCoreAddition;
        private readonly EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition;
        private readonly EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition;

        public EntityDetailPageHtmlGeneration(
            EntityCoreAddition frontendEntityCoreAddition,
            EntityDetailPageHtmlPropertyAddition entityDetailPageHtmlPropertyAddition,
            EntityDetailPageHtmlToPropertyAddition entityDetailPageHtmlToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityDetailPageHtmlPropertyAddition = entityDetailPageHtmlPropertyAddition;
            this.entityDetailPageHtmlToPropertyAddition = entityDetailPageHtmlToPropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntityToFrontend(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.entityDetailPageHtmlPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityDetailPageHtmlToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityDetailPageHtmlToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}