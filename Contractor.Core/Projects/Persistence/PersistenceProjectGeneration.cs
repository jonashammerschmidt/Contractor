using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Projects.Persistence;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Logic
{
    public class PersistenceProjectGeneration : IProjectGeneration
    {
        private static string ProjectFolder = ".Persistence";
        private static string DomainFolder = ".Persistence/Model/{Domain}";
        private static string DomainEfCoreFolder = ".Persistence/Model/{Domain}/EfCore";

        private static string TemplateFolder = Folder.Executable + @"\Projects\Persistence\Templates";
        private static string PersistenceRepositoryTemplateFileName = Path.Combine(TemplateFolder, "PersistenceRepositoryTemplate.txt");
        private static string PersistenceDbContextTemplateFileName = Path.Combine(TemplateFolder, "PersistenceDbContextTemplate.txt");
        private static string PersistenceDbDtoTemplateFileName = Path.Combine(TemplateFolder, "PersistenceDbDtoTemplate.txt");
        private static string PersistenceEfDtoTemplateFileName = Path.Combine(TemplateFolder, "PersistenceEfDtoTemplate.txt");

        private static string PersistenceRepositoryFileName = "EntitiesRepository.cs";
        private static string PersistenceDbContextFileName = "DomainDbContext.cs";
        private static string PersistenceDbDtoFileName = "DbEntity.cs";
        private static string PersistenceEfDtoFileName = "EfEntity.cs";

        private static string PersistenceDependencyProviderFileName = "DependencyProvider.cs";

        private DomainDependencyProvider domainDependencyProvider;
        private EntityCoreAddition entityCoreAddition;
        private EntityCoreDependencyProvider entityCoreDependencyProvider;
        private DbContextAddition dbContextAddition;
        private DbContextEntityAddition dbContextEntityAddition;
        private DbContextPropertyAddition dbContextPropertyAddition;
        private DbDtoMethodsAddition dbDtoMethodsAddition;
        private DtoAddition dtoAddition;
        private DtoPropertyAddition propertyAddition;
        private PathService pathService;

        public PersistenceProjectGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreAddition entityCoreAddition,
            EntityCoreDependencyProvider entityCoreDependencyProvider,
            DbContextAddition dbContextAddition,
            DbContextEntityAddition dbContextEntityAddition,
            DbContextPropertyAddition dbContextPropertyAddition,
            DbDtoMethodsAddition dbDtoMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            PathService pathService)
        {
            this.domainDependencyProvider = domainDependencyProvider;
            this.entityCoreAddition = entityCoreAddition;
            this.entityCoreDependencyProvider = entityCoreDependencyProvider;
            this.dbContextAddition = dbContextAddition;
            this.dbContextEntityAddition = dbContextEntityAddition;
            this.dbContextPropertyAddition = dbContextPropertyAddition;
            this.dbDtoMethodsAddition = dbDtoMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.pathService = pathService;
        }

        public void ClearDomain(DomainOptions options)
        {
            this.pathService.DeleteDomainFolder(options, DomainFolder);
        }

        public void AddDomain(DomainOptions options)
        {
            this.pathService.AddDomainFolder(options, DomainFolder);
            this.pathService.AddDtoFolder(options, DomainFolder);

            this.pathService.AddDomainFolder(options, DomainEfCoreFolder);
            this.pathService.AddDtoFolder(options, DomainEfCoreFolder);

            this.dbContextAddition.Add(options, DomainEfCoreFolder, PersistenceDbContextTemplateFileName, PersistenceDbContextFileName);

            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, PersistenceDependencyProviderFileName);
        }

        public void AddEntity(EntityOptions options)
        {

            string persistenceRepositoryTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, PersistenceRepositoryTemplateFileName);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, persistenceRepositoryTemplateFileName, PersistenceRepositoryFileName);

            string persistenceDbDtoTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, PersistenceDbDtoTemplateFileName);
            this.dtoAddition.AddDto(options, DomainFolder, persistenceDbDtoTemplateFileName, PersistenceDbDtoFileName);

            string persistenceEfDtoTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, PersistenceEfDtoTemplateFileName);
            this.dtoAddition.AddDto(options, DomainEfCoreFolder, persistenceEfDtoTemplateFileName, PersistenceEfDtoFileName);

            this.dbContextEntityAddition.Add(options, DomainEfCoreFolder, PersistenceDbContextFileName);

            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, PersistenceDependencyProviderFileName);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceDbDtoFileName);
            this.dbDtoMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoFileName);

            this.dbContextPropertyAddition.Add(options, DomainEfCoreFolder, PersistenceDbContextFileName);
            this.propertyAddition.AddPropertyToDTO(options, DomainEfCoreFolder, PersistenceEfDtoFileName);
        }
    }
}