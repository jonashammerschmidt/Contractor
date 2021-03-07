using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class PersistenceTestsProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = "Persistence.Tests\\Modules\\{Domain}\\{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Persistence.Tests\Templates";
        private static readonly string EntitiesCrudRepositoryTestsTemplateFileName = Path.Combine(TemplateFolder, "EntitiesCrudRepositoryTestsTemplate.txt");
        private static readonly string EntityTestValuesTemplateFileName = Path.Combine(TemplateFolder, "EntityTestValuesTemplate.txt");
        private static readonly string DbEntityDetailTestTemplateFileName = Path.Combine(TemplateFolder, "DbEntityDetailTestTemplate.txt");
        private static readonly string DbEntityTestTemplateFileName = Path.Combine(TemplateFolder, "DbEntityTestTemplate.txt");

        private static readonly string EntitiesCrudRepositoryTestsFileName = "EntitiesCrudRepositoryTests.cs";
        private static readonly string EntityTestValuesFileName = "EntityTestValues.cs";
        private static readonly string DbEntityDetailTestFileName = "DbEntityDetailTest.cs";
        private static readonly string DbEntityTestFileName = "DbEntityTest.cs";

        private readonly InMemoryDbContextEntityAddition inMemoryDbContextEntityAddition;
        private readonly DbDtoTestMethodsAddition dbDtoTestMethodsAddition;
        private readonly DbDtoDetailTestMethodsAddition dbDtoDetailTestMethodsAddition;
        private readonly DtoTestValuesAddition dtoTestValuesAddition;
        private readonly DtoTestValuesRelationAddition dtoTestValuesRelationAddition;
        private readonly DbDtoDetailTestFromAssertAddition dbDtoDetailTestFromAssertAddition;
        private readonly DbDtoDetailTestToAssertAddition dbDtoDetailTestToAssertAddition;
        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

        public PersistenceTestsProjectGeneration(
            InMemoryDbContextEntityAddition inMemoryDbContextEntityAddition,
            DbDtoTestMethodsAddition dbDtoTestMethodsAddition,
            DbDtoDetailTestMethodsAddition dbDtoDetailTestMethodsAddition,
            DtoTestValuesAddition dtoTestValuesAddition,
            DtoTestValuesRelationAddition dtoTestValuesRelationAddition,
            DbDtoDetailTestFromAssertAddition dbDtoDetailTestFromAssertAddition,
            DbDtoDetailTestToAssertAddition dbDtoDetailTestToAssertAddition,
            EntityCoreAddition entityCoreAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            PathService pathService)
        {
            this.inMemoryDbContextEntityAddition = inMemoryDbContextEntityAddition;
            this.dbDtoTestMethodsAddition = dbDtoTestMethodsAddition;
            this.dbDtoDetailTestMethodsAddition = dbDtoDetailTestMethodsAddition;
            this.dtoTestValuesAddition = dtoTestValuesAddition;
            this.dtoTestValuesRelationAddition = dtoTestValuesRelationAddition;
            this.dbDtoDetailTestFromAssertAddition = dbDtoDetailTestFromAssertAddition;
            this.dbDtoDetailTestToAssertAddition = dbDtoDetailTestToAssertAddition;
            this.entityCoreAddition = entityCoreAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.pathService = pathService;
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            // Crud Tests
            this.pathService.AddEntityFolder(options, DomainFolder);

            string entitiesCrudRepositoryTestsTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, EntitiesCrudRepositoryTestsTemplateFileName);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, entitiesCrudRepositoryTestsTemplateFileName, EntitiesCrudRepositoryTestsFileName);

            // Entity Test Values
            string entityTestValuesTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, EntityTestValuesTemplateFileName);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, entityTestValuesTemplateFileName, EntityTestValuesFileName);

            // DTOs
            this.pathService.AddDtoFolder(options, DomainFolder);

            this.dtoAddition.AddDto(options, DomainFolder, DbEntityDetailTestTemplateFileName, DbEntityDetailTestFileName);

            this.dtoAddition.AddDto(options, DomainFolder, DbEntityTestTemplateFileName, DbEntityTestFileName);

            this.inMemoryDbContextEntityAddition.Add(options);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.dtoTestValuesAddition.Add(options, DomainFolder, EntityTestValuesFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, DbEntityDetailTestFileName);
            this.dbDtoDetailTestMethodsAddition.Add(options, DomainFolder, DbEntityDetailTestFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, DbEntityTestFileName);
            this.dbDtoTestMethodsAddition.Add(options, DomainFolder, DbEntityTestFileName);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IPropertyAdditionOptions dbFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");
            this.propertyAddition.AddPropertyToDTO(dbFromOptions, DomainFolder, DbEntityDetailTestFileName);
            this.dbDtoDetailTestFromAssertAddition.Add(options, DomainFolder, DbEntityDetailTestFileName);

            // To
            this.dtoTestValuesRelationAddition.Add(options, DomainFolder, EntityTestValuesFileName);

            IPropertyAdditionOptions dbToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", $"{options.EntityNameFrom}");
            this.propertyAddition.AddPropertyToDTO(dbToOptions, DomainFolder, DbEntityDetailTestFileName);
            this.dbDtoDetailTestToAssertAddition.Add(options, DomainFolder, DbEntityDetailTestFileName);

            IPropertyAdditionOptions guidPropertyOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.propertyAddition.AddPropertyToDTO(guidPropertyOptions, DomainFolder, DbEntityTestFileName);
            this.dbDtoTestMethodsAddition.Add(guidPropertyOptions, DomainFolder, DbEntityTestFileName);
        }
    }
}