﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityUpdateTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "DbEntityUpdateTestTemplate.txt");

        private static readonly string FileName = "DbEntityUpdateTest.cs";

        private readonly DbEntityUpdateTestMethodsAddition dbUpdateDtoTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityUpdateTestGeneration(
            DbEntityUpdateTestMethodsAddition dbDtoTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbUpdateDtoTestMethodsAddition = dbDtoTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        public DbEntityUpdateTestGeneration()
        {
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, PersistenceTestsProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbUpdateDtoTestMethodsAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions guidPropertyOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(guidPropertyOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(guidPropertyOptions);
            this.dbUpdateDtoTestMethodsAddition.Add(propertyAdditionOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}