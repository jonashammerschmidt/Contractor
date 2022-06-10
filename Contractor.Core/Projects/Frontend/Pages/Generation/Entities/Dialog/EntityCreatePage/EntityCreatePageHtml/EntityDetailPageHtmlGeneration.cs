using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityCreatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-create.page.html.template.txt");

        private static readonly string FileName = "dialogs\\create\\entity-kebab-create.dialog.html";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityCreatePageHtmlPropertyAddition entityCreatePageHtmlPropertyAddition;
        private readonly EntityCreatePageHtmlToPropertyAddition entityCreatePageHtmlToPropertyAddition;

        public EntityCreatePageHtmlGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityCreatePageHtmlPropertyAddition entityCreatePageHtmlPropertyAddition,
            EntityCreatePageHtmlToPropertyAddition entityCreatePageHtmlToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityCreatePageHtmlPropertyAddition = entityCreatePageHtmlPropertyAddition;
            this.entityCreatePageHtmlToPropertyAddition = entityCreatePageHtmlToPropertyAddition;
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
            this.entityCreatePageHtmlPropertyAddition.Edit(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.entityCreatePageHtmlToPropertyAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            this.Add1ToNRelation(new Relation1ToN(relation));
        }
    }
}