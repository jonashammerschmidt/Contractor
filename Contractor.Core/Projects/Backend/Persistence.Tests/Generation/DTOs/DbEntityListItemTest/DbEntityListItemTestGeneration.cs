using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityListItemTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "DbEntityListItemTestTemplate.txt");

        private static readonly string FileName = "DbEntityListItemTest.cs";

        private readonly DbEntityListItemTestMethodsAddition dbDtoListItemTestMethodsAddition;
        private readonly DbEntityListItemTestToAssertAddition dbDtoListItemTestToAssertAddition;
        private readonly DbEntityListItemTestFromOneToOneAssertAddition dbEntityListItemTestFromOneToOneAssertAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityListItemTestGeneration(
            DbEntityListItemTestMethodsAddition dbDtoListItemTestMethodsAddition,
            DbEntityListItemTestToAssertAddition dbDtoListItemTestToAssertAddition,
            DbEntityListItemTestFromOneToOneAssertAddition dbEntityListItemTestFromOneToOneAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoListItemTestMethodsAddition = dbDtoListItemTestMethodsAddition;
            this.dbDtoListItemTestToAssertAddition = dbDtoListItemTestToAssertAddition;
            this.dbEntityListItemTestFromOneToOneAssertAddition = dbEntityListItemTestFromOneToOneAssertAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
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
            this.dbDtoListItemTestMethodsAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions dbToOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbToOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbDtoListItemTestToAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions dbFromOptions =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbFromOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbEntityListItemTestFromOneToOneAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);

            // To
            IRelationSideAdditionOptions dbToOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbToOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbDtoListItemTestToAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}