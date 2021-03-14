using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class IEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-detail.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-detail.ts";

        private readonly FrontendDtoAddition frontendDtoAddition;

        public IEntityDetailGeneration(FrontendDtoAddition frontendDtoAddition)
        {
            this.frontendDtoAddition = frontendDtoAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendDtoAddition.AddDto(options, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}