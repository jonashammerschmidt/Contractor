using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityUpdatePageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.ts.template.txt");

        private static readonly string FileName = "dialogs\\update\\entity-kebab-update.dialog.ts";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntityUpdatePageTsPropertyAddition entityUpdatePageTsPropertyAddition;
        private readonly EntityUpdatePageTsToPropertyAddition entityUpdatePageTsToPropertyAddition;

        public EntityUpdatePageTsGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntityUpdatePageTsPropertyAddition entityUpdatePageTsPropertyAddition,
            EntityUpdatePageTsToPropertyAddition entityUpdatePageTsToPropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entityUpdatePageTsPropertyAddition = entityUpdatePageTsPropertyAddition;
            this.entityUpdatePageTsToPropertyAddition = entityUpdatePageTsToPropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendPagesEntityCoreAddition.AddEntityCore(options, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.entityUpdatePageTsPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entityUpdatePageTsToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entityUpdatePageTsToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}