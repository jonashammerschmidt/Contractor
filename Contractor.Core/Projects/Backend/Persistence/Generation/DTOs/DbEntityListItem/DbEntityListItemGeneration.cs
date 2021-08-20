﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityListItemTemplate.txt");

        private static readonly string FileName = "DbEntityListItem.cs";

        private readonly DbEntityListItemMethodsAddition dbDtoListItemMethodsAddition;
        private readonly DbEntityListItemToMethodsAddition dbDtoListItemToMethodsAddition;
        private readonly DbEntityListItemFromOneToOneMethodsAddition dbEntityListItemFromOneToOneMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityListItemGeneration(
            DbEntityListItemMethodsAddition dbDtoListItemMethodsAddition,
            DbEntityListItemToMethodsAddition dbDtoListItemToMethodsAddition,
            DbEntityListItemFromOneToOneMethodsAddition dbEntityListItemFromOneToOneMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoListItemMethodsAddition = dbDtoListItemMethodsAddition;
            this.dbDtoListItemToMethodsAddition = dbDtoListItemToMethodsAddition;
            this.dbEntityListItemFromOneToOneMethodsAddition = dbEntityListItemFromOneToOneMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, PersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceProjectGeneration.DomainFolder, FileName);
            this.dbDtoListItemMethodsAddition.Add(options, PersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.dbDtoListItemToMethodsAddition.Add(options, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IDb{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(optionsFrom, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            this.dbEntityListItemFromOneToOneMethodsAddition.Add(options, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.dbDtoListItemToMethodsAddition.Add(options, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}