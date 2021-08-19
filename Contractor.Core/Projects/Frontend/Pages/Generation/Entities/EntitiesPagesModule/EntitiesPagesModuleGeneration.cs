using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPagesModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab-pages.module.template.txt");

        private static readonly string FileName = "entities-kebab-pages.module.ts";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;
        private readonly EntitiesPagesModuleToRelationAddition entitiesPagesModuleToRelationAddition;

        public EntitiesPagesModuleGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition,
            EntitiesPagesModuleToRelationAddition entitiesPagesModuleToRelationAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
            this.entitiesPagesModuleToRelationAddition = entitiesPagesModuleToRelationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
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
            this.entitiesPagesModuleToRelationAddition.Add(options, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}