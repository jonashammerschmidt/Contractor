using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class EntitiesCrudServiceGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "entities-kebab-crud.service.template.txt");

        private static readonly string FileName = "entities-kebab-crud.service.ts";

        private readonly FrontendModelEntityCoreAddition frontendModelEntityCoreAddition;

        public EntitiesCrudServiceGeneration(
            FrontendModelEntityCoreAddition frontendModelEntityCoreAddition)
        {
            this.frontendModelEntityCoreAddition = frontendModelEntityCoreAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
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