﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class IEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-list-item.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-list-item.ts";

        private readonly FrontendDtoAddition frontendDtoAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoRelationAddition frontendDtoRelationAddition;
        private readonly FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition;

        public IEntityListItemGeneration(
            FrontendDtoAddition frontendDtoAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoRelationAddition frontendDtoRelationAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition)
        {
            this.frontendDtoAddition = frontendDtoAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoRelationAddition = frontendDtoRelationAddition;
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
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            string toImportStatementPath = $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}";

            IRelationSideAdditionOptions toOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");

            this.frontendDtoRelationAddition.AddPropertyToDTO(toOptions, ModelProjectGeneration.DomainFolder, FileName,
                $"I{options.EntityNameFrom}", toImportStatementPath);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            string fromImportStatementPath = $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameTo)}";

            IRelationSideAdditionOptions fromOptions =
                RelationAdditionOptions.GetPropertyForFrom(options, $"I{options.EntityNameTo}");

            this.frontendDtoRelationAddition.AddPropertyToDTO(fromOptions, ModelProjectGeneration.DomainFolder, FileName,
                $"I{options.EntityNameTo}", fromImportStatementPath);

            // To
            string toImportStatementPath = $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}";

            IRelationSideAdditionOptions toOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");

            this.frontendDtoRelationAddition.AddPropertyToDTO(toOptions, ModelProjectGeneration.DomainFolder, FileName,
                $"I{options.EntityNameFrom}", toImportStatementPath);
        }
    }
}