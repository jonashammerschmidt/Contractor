using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Contract
{
    public class ContractPersistenceProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = ".Contract/Persistence/Model/{Domain}/{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Contract.Persistence\Templates";

        private static readonly string IPersistenceDbDtoTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceDbDtoTemplate.txt");
        private static readonly string IPersistenceRepositoryTemplateFileName = Path.Combine(TemplateFolder, "IPersistenceRepositoryTemplate.txt");

        private static readonly string IPersistenceDbDtoFileName = "IDbEntity.cs";
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

        public void AddDomain(DomainOptions options)
        {
        }

        public void AddEntity(EntityOptions options)
        {
            this.pathService.AddEntityFolder(options, DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, IPersistenceRepositoryTemplateFileName, IPersistenceRepositoryFileName);

            this.pathService.AddDtoFolder(options, DomainFolder);
            this.dtoAddition.AddDto(options, DomainFolder, IPersistenceDbDtoTemplateFileName, IPersistenceDbDtoFileName);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, IPersistenceDbDtoFileName, true);
        }
    }
}