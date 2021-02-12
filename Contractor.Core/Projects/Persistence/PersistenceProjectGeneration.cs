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
        private static readonly string DomainFolder = ".Persistence\\Model\\{Domain}\\{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Persistence\Templates";
        private static readonly string PersistenceRepositoryTemplateFileName = Path.Combine(TemplateFolder, "PersistenceRepositoryTemplate.txt");
        private static readonly string PersistenceDbDtoTemplateFileName = Path.Combine(TemplateFolder, "PersistenceDbDtoTemplate.txt");
        private static readonly string PersistenceDbDtoDetailTemplateFileName = Path.Combine(TemplateFolder, "PersistenceDbDtoDetailTemplate.txt");
        private static readonly string PersistenceEfDtoTemplateFileName = Path.Combine(TemplateFolder, "PersistenceEfDtoTemplate.txt");

        private static readonly string PersistenceRepositoryFileName = "EntitiesCrudRepository.cs";
        private static readonly string PersistenceDbDtoFileName = "DbEntity.cs";
        private static readonly string PersistenceDbDtoDetailFileName = "DbEntityDetail.cs";
        private static readonly string PersistenceEfDtoFileName = "EfEntity.cs";

        private static readonly string PersistenceDependencyProviderFileName = "DependencyProvider.cs";

        private readonly DomainDependencyProvider domainDependencyProvider;
        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntityCoreDependencyProvider entityCoreDependencyProvider;
        private readonly DbContextEntityAddition dbContextEntityAddition;
        private readonly DbContextPropertyAddition dbContextPropertyAddition;
        private readonly DbContextRelationToAddition dbContextRelationToAddition;
        private readonly DbDtoMethodsAddition dbDtoMethodsAddition;
        private readonly DbDtoDetailMethodsAddition dbDtoDetailMethodsAddition;
        private readonly DbDtoDetailFromMethodsAddition dbDtoDetailFromMethodsAddition;
        private readonly DbDtoDetailToMethodsAddition dbDtoDetailToMethodsAddition;
        private readonly EfDtoContructorHashSetAddition efDtoContructorHashSetAddition;
        private readonly DtoFromRepositoryIncludeAddition dtoFromRepositoryIncludeAddition;
        private readonly DtoToRepositoryIncludeAddition dtoToRepositoryIncludeAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

        public PersistenceProjectGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreAddition entityCoreAddition,
            EntityCoreDependencyProvider entityCoreDependencyProvider,
            DbContextEntityAddition dbContextEntityAddition,
            DbContextPropertyAddition dbContextPropertyAddition,
            DbContextRelationToAddition dbContextRelationToAddition,
            DbDtoMethodsAddition dbDtoMethodsAddition,
            DbDtoDetailMethodsAddition dbDtoDetailMethodsAddition,
            DbDtoDetailFromMethodsAddition dbDtoDetailFromMethodsAddition,
            DbDtoDetailToMethodsAddition dbDtoDetailToMethodsAddition,
            EfDtoContructorHashSetAddition efDtoContructorHashSetAddition,
            DtoFromRepositoryIncludeAddition dtoFromRepositoryIncludeAddition,
            DtoToRepositoryIncludeAddition dtoToRepositoryIncludeAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            PathService pathService)
        {
            this.domainDependencyProvider = domainDependencyProvider;
            this.entityCoreAddition = entityCoreAddition;
            this.entityCoreDependencyProvider = entityCoreDependencyProvider;
            this.dbContextEntityAddition = dbContextEntityAddition;
            this.dbContextPropertyAddition = dbContextPropertyAddition;
            this.dbContextRelationToAddition = dbContextRelationToAddition;
            this.dbDtoMethodsAddition = dbDtoMethodsAddition;
            this.dbDtoDetailMethodsAddition = dbDtoDetailMethodsAddition;
            this.dbDtoDetailFromMethodsAddition = dbDtoDetailFromMethodsAddition;
            this.dbDtoDetailToMethodsAddition = dbDtoDetailToMethodsAddition;
            this.efDtoContructorHashSetAddition = efDtoContructorHashSetAddition;
            this.dtoFromRepositoryIncludeAddition = dtoFromRepositoryIncludeAddition;
            this.dtoToRepositoryIncludeAddition = dtoToRepositoryIncludeAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.pathService = pathService;
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, PersistenceDependencyProviderFileName);
        }

        public void AddEntity(IEntityAdditionOptions options)
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

            this.dtoAddition.AddDto(options, DomainFolder, PersistenceDbDtoDetailTemplateFileName, PersistenceDbDtoDetailFileName);

            this.dbContextEntityAddition.Add(options);

            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, PersistenceDependencyProviderFileName);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceDbDtoFileName);
            this.dbDtoMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceEfDtoFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, PersistenceDbDtoDetailFileName);
            this.dbDtoDetailMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoDetailFileName);

            this.dbContextPropertyAddition.Add(options);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForFrom(options, $"virtual ICollection<Ef{options.EntityNameTo}>", options.EntityNamePluralTo),
                DomainFolder, PersistenceEfDtoFileName);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", options.EntityNamePluralTo),
                DomainFolder, PersistenceDbDtoDetailFileName,
                $"{options.ProjectName}.Contract.Persistence.Model.{options.DomainTo}.{options.EntityNamePluralTo}");
            this.dbDtoDetailFromMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoDetailFileName,
                $"{options.ProjectName}.Persistence.Model.{options.DomainTo}.{options.EntityNamePluralTo}");

            this.efDtoContructorHashSetAddition.Add(options, DomainFolder, PersistenceEfDtoFileName,
                $"{options.ProjectName}.Persistence.Model.{options.DomainTo}.{options.EntityNamePluralTo}");

            this.dtoFromRepositoryIncludeAddition.Add(options, DomainFolder, PersistenceRepositoryFileName);

            // To
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, PersistenceEfDtoFileName);
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, $"virtual Ef{options.EntityNameFrom}", options.EntityNameFrom),
                DomainFolder, PersistenceEfDtoFileName,
                $"{options.ProjectName}.Persistence.Model.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, PersistenceDbDtoFileName);
            this.dbDtoMethodsAddition.Add(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, PersistenceDbDtoFileName);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", options.EntityNameFrom),
                DomainFolder, PersistenceDbDtoDetailFileName,
                $"{options.ProjectName}.Contract.Persistence.Model.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dbDtoDetailToMethodsAddition.Add(options, DomainFolder, PersistenceDbDtoDetailFileName,
                $"{options.ProjectName}.Persistence.Model.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.dbContextRelationToAddition.Add(options);

            this.dtoToRepositoryIncludeAddition.Add(options, DomainFolder, PersistenceRepositoryFileName);
        }
    }
}