using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntitiesPagesModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab-pages.module.template.txt");

        private static readonly string FileName = "entities-kebab-pages.module.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntitiesPagesModuleToRelationAddition entitiesPagesModuleToRelationAddition;

        public EntitiesPagesModuleGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntitiesPagesModuleToRelationAddition entitiesPagesModuleToRelationAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entitiesPagesModuleToRelationAddition = entitiesPagesModuleToRelationAddition;
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
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.entitiesPagesModuleToRelationAddition.Edit(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}