using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class EntitiesModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "entities-kebab.module.template.txt");

        private static readonly string FileName = "entities-kebab.module.ts";

        private readonly FrontendModelEntityCoreAddition frontendModelEntityCoreAddition;

        public EntitiesModuleGeneration(
            FrontendModelEntityCoreAddition frontendModelEntityCoreAddition)
        {
            this.frontendModelEntityCoreAddition = frontendModelEntityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendModelEntityCoreAddition.AddEntityCore(options, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}