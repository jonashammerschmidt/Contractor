﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    internal class IDbEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractPersistenceProjectGeneration.TemplateFolder, "IDbEntityListItemTemplate.txt");

        private static readonly string FileName = "IDbEntityListItem.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IDbEntityListItemGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, ContractPersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, ContractPersistenceProjectGeneration.DomainFolder, FileName, true);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions optionsTo = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, ContractPersistenceProjectGeneration.DomainFolder, FileName, true,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}