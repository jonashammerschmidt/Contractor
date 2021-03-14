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
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition;

        public IEntityDetailGeneration(
            FrontendDtoAddition frontendDtoAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition)
        {
            this.frontendDtoAddition = frontendDtoAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoPropertyMethodAddition = frontendDtoPropertyMethodAddition;
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
            this.frontendDtoPropertyAddition.AddPropertyToDTO(options, ModelProjectGeneration.DomainFolder, FileName);

            this.frontendDtoPropertyMethodAddition.AddPropertyToDTO(options, "fromApiEntityDetail", "apiEntityDetail", ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}