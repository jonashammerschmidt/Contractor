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

        public DbEntityDetailTestGeneration(
            DbEntityDetailTestMethodsAddition dbDtoDetailTestMethodsAddition,
            DbEntityDetailTestFromAssertAddition dbDtoDetailTestFromAssertAddition,
            DbEntityDetailTestToAssertAddition dbDtoDetailTestToAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dbDtoDetailTestMethodsAddition = dbDtoDetailTestMethodsAddition;
            this.dbDtoDetailTestFromAssertAddition = dbDtoDetailTestFromAssertAddition;
            this.dbDtoDetailTestToAssertAddition = dbDtoDetailTestToAssertAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
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
            IPropertyAdditionOptions dbFromOptions =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");
            this.propertyAddition.AddPropertyToDTO(dbFromOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbDtoDetailTestFromAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);

            // To
            IPropertyAdditionOptions dbToOptions =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", $"{options.EntityNameFrom}");
            this.propertyAddition.AddPropertyToDTO(dbToOptions, PersistenceTestsProjectGeneration.DomainFolder, FileName);
            this.dbDtoDetailTestToAssertAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}