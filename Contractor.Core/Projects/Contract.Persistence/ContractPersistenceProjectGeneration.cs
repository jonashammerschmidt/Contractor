using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Contract
{
    public class ContractPersistenceProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = ".Contract\\Persistence\\Model\\{Domain}\\{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Contract.Persistence\Templates";

        private static readonly string IPersistenceDbDtoTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceDbDtoTemplate.txt");
        private static readonly string IPersistenceDbDtoDetailTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceDbDtoDetailTemplate.txt");
        private static readonly string IPersistenceRepositoryTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceRepositoryTemplate.txt");

        private static readonly string IPersistenceDbDtoFileName = "IDbEntity.cs";
        private static readonly string IPersistenceDbDtoDetailFileName = "IDbEntityDetail.cs";
        private static readonly string IPersistenceRepositoryFileName = "IEntitiesCrudRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

        public ContractPersistenceProjectGeneration(
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
            this.pathService.AddEntityFolder(options, DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, IPersistenceRepositoryTemplateFileName, IPersistenceRepositoryFileName);

            this.pathService.AddDtoFolder(options, DomainFolder);
            this.dtoAddition.AddDto(options, DomainFolder, IPersistenceDbDtoTemplateFileName, IPersistenceDbDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, IPersistenceDbDtoDetailTemplateFileName, IPersistenceDbDtoDetailFileName);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, IPersistenceDbDtoFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, IPersistenceDbDtoDetailFileName, true);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", $"{options.EntityNamePluralTo}"),
                DomainFolder, IPersistenceDbDtoDetailFileName, true,
                $"{options.ProjectName}.Contract.Persistence.Model.{options.DomainTo}.{options.EntityNamePluralTo}");

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, IPersistenceDbDtoFileName, true);
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", options.EntityNameFrom),
                DomainFolder, IPersistenceDbDtoDetailFileName, true,
                $"{options.ProjectName}.Contract.Persistence.Model.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}