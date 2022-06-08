using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityDetailPageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.ts.template.txt");

        private static readonly string FileName = "sub-pages\\detail\\entity-kebab-detail.page.ts";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntityDetailPageTsFromPropertyAddition entityDetailPageTsFromPropertyAddition;

        public EntityDetailPageTsGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntityDetailPageTsFromPropertyAddition entityDetailPageTsFromPropertyAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entityDetailPageTsFromPropertyAddition = entityDetailPageTsFromPropertyAddition;
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
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entityDetailPageTsFromPropertyAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}