using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class IEntityCreateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-create.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-create.ts";

        private readonly FrontendDtoAddition frontendDtoAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition;
        private readonly FrontendDtoPropertyDefaultAddition frontendDtoPropertyDefaultAddition;

        public IEntityCreateGeneration(
            FrontendDtoAddition frontendDtoAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition,
            FrontendDtoPropertyDefaultAddition frontendDtoPropertyDefaultAddition)
        {
            this.frontendDtoAddition = frontendDtoAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoPropertyMethodAddition = frontendDtoPropertyMethodAddition;
            this.frontendDtoPropertyDefaultAddition = frontendDtoPropertyDefaultAddition;
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

            this.frontendDtoPropertyMethodAddition.AddPropertyToDTO(options, "toApiEntityCreate", "entityCreate", ModelProjectGeneration.DomainFolder, FileName);

            this.frontendDtoPropertyDefaultAddition.AddPropertyToDTO(options, ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IPropertyAdditionOptions toOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameLowerFrom}Id");
            this.frontendDtoPropertyAddition.AddPropertyToDTO(toOptions, ModelProjectGeneration.DomainFolder, FileName);

            this.frontendDtoPropertyMethodAddition.AddPropertyToDTO(toOptions, "toApiEntityCreate", "entityCreate", ModelProjectGeneration.DomainFolder, FileName);

            this.frontendDtoPropertyDefaultAddition.AddPropertyToDTO(toOptions, ModelProjectGeneration.DomainFolder, FileName);
        }
    }
}