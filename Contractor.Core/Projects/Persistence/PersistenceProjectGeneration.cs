using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Projects.Persistence;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Logic
{
    public class PersistenceProjectGeneration : IProjectGeneration
    {
        private static readonly string ProjectFolder = ".Persistence";
        private static readonly string DomainFolder = ".Persistence/Model/{Domain}/{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Persistence\Templates";
        private static readonly string PersistenceRepositoryTemplateFileName = Path.Combine(TemplateFolder, "PersistenceRepositoryTemplate.txt");
        private static readonly string PersistenceDbDtoTemplateFileName = Path.Combine(TemplateFolder, "PersistenceDbDtoTemplate.txt");
        private static readonly string PersistenceEfDtoTemplateFileName = Path.Combine(TemplateFolder, "PersistenceEfDtoTemplate.txt");

        private static readonly string PersistenceRepositoryFileName = "EntitiesCrudRepository.cs";
        private static readonly string PersistenceDbDtoFileName = "DbEntity.cs";
        private static readonly string PersistenceEfDtoFileName = "EfEntity.cs";

        private static readonly string PersistenceDependencyProviderFileName = "DependencyProvider.cs";

        private readonly DomainDependencyProvider domainDependencyProvider;
        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntityCoreDependencyProvider entityCoreDependencyProvider;
        private readonly DbContextEntityAddition dbContextEntityAddition;
        private readonly DbContextPropertyAddition dbContextPropertyAddition;
        private readonly DbDtoMethodsAddition dbDtoMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

        public PersistenceProjectGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreAddition entityCoreAddition,
            EntityCoreDependencyProvider entityCoreDependencyProvider,
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
            this.dbContextEntityAddition = dbContextEntityAddition;
            this.dbContextPropertyAddition = dbContextPropertyAddition;
            this.dbDtoMethodsAddition = dbDtoMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.pathService = pathService;
        }

        public void AddDomain(DomainOptions options)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, PersistenceDependencyProviderFileName);
        }

        public void AddEntity(EntityOptions options)
        {
            // Entity Core
            this.pathService.AddEntityFolder(options, DomainFolder);
            
            string persistenceRepositoryTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, PersistenceRepositoryTemplateFileName);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, persistenceRepositoryTemplateFileName, PersistenceRepositoryFileName);

            // DTOs
            this.pathService.AddDtoFolder(options, DomainFolder);

            string persistenceDbDtoTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, PersistenceDbDtoTemplateFileName);
            this.dtoAddition.AddDto(options, DomainFolder, persistenceDbDtoTemplateFileName, PersistenceDbDtoFileName);

            string persistenceEfDtoTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, PersistenceEfDtoTemplateFileName);
            this.dtoAddition.AddDto(options, DomainFolder, persistenceEfDtoTemplateFileName, PersistenceEfDtoFileName);

            this.dbContextEntityAddition.Add(options);

            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, PersistenceDependencyProviderFileName);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceDbDtoFileName);
            this.dbDtoMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoFileName);

            this.dbContextPropertyAddition.Add(options);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceEfDtoFileName);
        }
    }
}