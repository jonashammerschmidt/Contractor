using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Contract
{
    public class ContractPersistenceProjectGeneration : IProjectGeneration
    {
        private static string DomainFolder = ".Contract/Persistence/Model/{Domain}";

        private static string TemplateFolder = Folder.Executable + @"\Projects\Contract.Persistence\Templates";

        private static string IPersistenceDbDtoTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceDbDtoTemplate.txt");
        private static string IPersistenceRepositoryTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceRepositoryTemplate.txt");

        private static string IPersistenceDbDtoFileName = "IDbEntity.cs";
        private static string IPersistenceRepositoryFileName = "IEntitiesRepository.cs";

        private EntityCoreAddition entityCoreAddition;
        private DtoAddition dtoAddition;
        private DtoPropertyAddition propertyAddition;
        private PathService pathService;

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

        public void ClearDomain(DomainOptions options)
        {
            this.pathService.DeleteDomainFolder(options, DomainFolder);
        }

        public void AddDomain(DomainOptions options)
        {
            this.pathService.AddDomainFolder(options, DomainFolder);
            this.pathService.AddDtoFolder(options, DomainFolder);
        }

        public void AddEntity(EntityOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, IPersistenceRepositoryTemplateFileName, IPersistenceRepositoryFileName);

            this.dtoAddition.AddDto(options, DomainFolder, IPersistenceDbDtoTemplateFileName, IPersistenceDbDtoFileName);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, IPersistenceDbDtoFileName, true);
        }
    }
}