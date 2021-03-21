using Contractor.Core.Helpers;
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
        private readonly FrontendDtoPropertyFromMethodAddition frontendDtoPropertyFromMethodAddition;
        private readonly FrontendDtoPropertyToMethodAddition frontendDtoPropertyToMethodAddition;

        public IEntityDetailGeneration(
            FrontendDtoAddition frontendDtoAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition,
            FrontendDtoPropertyFromMethodAddition frontendDtoPropertyFromMethodAddition,
            FrontendDtoPropertyToMethodAddition frontendDtoPropertyToMethodAddition)
        {
            this.frontendDtoAddition = frontendDtoAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoPropertyMethodAddition = frontendDtoPropertyMethodAddition;
            this.frontendDtoPropertyFromMethodAddition = frontendDtoPropertyFromMethodAddition;
            this.frontendDtoPropertyToMethodAddition = frontendDtoPropertyToMethodAddition;
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
            // From
            string fromImportStatementPath = $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameTo)}";

            IPropertyAdditionOptions fromOptions =
                RelationAdditionOptions.GetPropertyForFrom(options, $"I{options.EntityNameTo}[]", $"{options.EntityNamePluralLowerTo}");

            this.frontendDtoPropertyAddition.AddPropertyToDTO(fromOptions, ModelProjectGeneration.DomainFolder, FileName,
                $"fromApi{options.EntityNameTo}, I{options.EntityNameTo}", fromImportStatementPath);

            frontendDtoPropertyFromMethodAddition.AddPropertyToDTO(options, ModelProjectGeneration.DomainFolder, FileName);

            // To
            string toImportStatementPath = $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}";

            IPropertyAdditionOptions toOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}", $"{options.EntityNameLowerFrom}");

            this.frontendDtoPropertyAddition.AddPropertyToDTO(toOptions, ModelProjectGeneration.DomainFolder, FileName,
                $"fromApi{options.EntityNameFrom}, I{options.EntityNameFrom}", toImportStatementPath);

            frontendDtoPropertyToMethodAddition.AddPropertyToDTO(options, ModelProjectGeneration.DomainFolder, FileName);
        }
    }
}