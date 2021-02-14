using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class PersistenceTestsProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = ".Persistence.Tests\\Model\\{Domain}\\{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Persistence.Tests\Templates";
        private static readonly string EntitiesCrudRepositoryTestsTemplateFileName = Path.Combine(TemplateFolder, "EntitiesCrudRepositoryTestsTemplate.txt");
        private static readonly string EntityTestValuesTemplateFileName = Path.Combine(TemplateFolder, "EntityTestValuesTemplate.txt");
        private static readonly string DbEntityDetailTestTemplateTemplateFileName = Path.Combine(TemplateFolder, "DbEntityDetailTestTemplate.txt");
        private static readonly string DbEntityTestTemplateTemplateFileName = Path.Combine(TemplateFolder, "DbEntityTestTemplate.txt");

        private static readonly string EntitiesCrudRepositoryTestsFileName = "EntitiesCrudRepositoryTests.cs";
        private static readonly string EntityTestValuesFileName = "EntityTestValues.cs";
        private static readonly string DbEntityDetailTestTemplateFileName = "DbEntityDetailTest.cs";
        private static readonly string DbEntityTestTemplateFileName = "DbEntityTest.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

        public PersistenceTestsProjectGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            PathService pathService)
        {
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

            this.entityCoreAddition.AddEntityCore(options, DomainFolder, EntitiesCrudRepositoryTestsTemplateFileName, EntitiesCrudRepositoryTestsFileName);

            // Entity Test Values
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, EntityTestValuesTemplateFileName, EntityTestValuesFileName);

            // DTOs
            this.pathService.AddDtoFolder(options, DomainFolder);

            this.dtoAddition.AddDto(options, DomainFolder, DbEntityDetailTestTemplateTemplateFileName, DbEntityDetailTestTemplateFileName);

            this.dtoAddition.AddDto(options, DomainFolder, DbEntityTestTemplateTemplateFileName, DbEntityTestTemplateFileName);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            //this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceDbDtoFileName);
            //this.dbDtoMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoFileName);

            //this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceEfDtoFileName);

            //this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceDbDtoDetailFileName);
            //this.dbDtoDetailMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoDetailFileName);

            //this.dbContextPropertyAddition.Add(options);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}