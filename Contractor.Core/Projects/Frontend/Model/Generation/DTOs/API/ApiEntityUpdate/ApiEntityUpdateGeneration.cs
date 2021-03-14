using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class ApiEntityUpdateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "api-entity-kebab-update.template.txt");

        private static readonly string FileName = "dtos\\api\\api-entity-kebab-update.ts";

        private readonly FrontendDtoAddition frontendDtoAddition;

        public ApiEntityUpdateGeneration(FrontendDtoAddition frontendDtoAddition)
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