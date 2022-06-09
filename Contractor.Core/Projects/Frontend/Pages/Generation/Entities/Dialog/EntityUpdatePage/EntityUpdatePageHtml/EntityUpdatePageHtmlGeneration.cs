using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityUpdatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.html.template.txt");

        private static readonly string FileName = "dialogs\\update\\entity-kebab-update.dialog.html";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityUpdatePageHtmlPropertyAddition entityUpdatePageHtmlPropertyAddition;
        private readonly EntityUpdatePageHtmlToPropertyAddition entityUpdatePageHtmlToPropertyAddition;

        public EntityUpdatePageHtmlGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityUpdatePageHtmlPropertyAddition entityUpdatePageHtmlPropertyAddition,
            EntityUpdatePageHtmlToPropertyAddition entityUpdatePageHtmlToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityUpdatePageHtmlPropertyAddition = entityUpdatePageHtmlPropertyAddition;
            this.entityUpdatePageHtmlToPropertyAddition = entityUpdatePageHtmlToPropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntity(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.entityUpdatePageHtmlPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entityUpdatePageHtmlToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}