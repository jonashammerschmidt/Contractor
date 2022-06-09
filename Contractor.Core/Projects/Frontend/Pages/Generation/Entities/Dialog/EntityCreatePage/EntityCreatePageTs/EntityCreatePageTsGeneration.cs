using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityCreatePageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-create.page.ts.template.txt");

        private static readonly string FileName = "dialogs\\create\\entity-kebab-create.dialog.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityCreatePageTsPropertyAddition entityCreatePageTsPropertyAddition;
        private readonly EntityCreatePageTsToPropertyAddition entityCreatePageTsToPropertyAddition;

        public EntityCreatePageTsGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityCreatePageTsPropertyAddition entityCreatePageTsPropertyAddition,
            EntityCreatePageTsToPropertyAddition entityCreatePageTsToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityCreatePageTsPropertyAddition = entityCreatePageTsPropertyAddition;
            this.entityCreatePageTsToPropertyAddition = entityCreatePageTsToPropertyAddition;
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
            this.entityCreatePageTsPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entityCreatePageTsToPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}