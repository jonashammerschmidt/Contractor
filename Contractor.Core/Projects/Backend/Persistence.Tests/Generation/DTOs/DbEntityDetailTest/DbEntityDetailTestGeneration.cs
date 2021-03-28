using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityDetailTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "DbEntityDetailTestTemplate.txt");

        private static readonly string FileName = "DbEntityDetailTest.cs";

        private readonly DbEntityDetailTestMethodsAddition dbDtoDetailTestMethodsAddition;
        private readonly DbEntityDetailTestFromAssertAddition dbDtoDetailTestFromAssertAddition;
        private readonly DbEntityDetailTestToAssertAddition dbDtoDetailTestToAssertAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityDetailTestGeneration(
            DbEntityDetailTestMethodsAddition dbDtoDetailTestMethodsAddition,
            DbEntityDetailTestFromAssertAddition dbDtoDetailTestFromAssertAddition,
            DbEntityDetailTestToAssertAddition dbDtoDetailTestToAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoDetailTestMethodsAddition = dbDtoDetailTestMethodsAddition;
            this.dbDtoDetailTestFromAssertAddition = dbDtoDetailTestFromAssertAddition;
            this.dbDtoDetailTestToAssertAddition = dbDtoDetailTestToAssertAddition;
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
            this.dbDtoDetailTestMethodsAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions dbFromOptions =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");
            this.relationAddition.AddRelationToDTO(dbFromOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbDtoDetailTestFromAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);

            // To
            IRelationSideAdditionOptions dbToOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", $"{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbToOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbDtoDetailTestToAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}