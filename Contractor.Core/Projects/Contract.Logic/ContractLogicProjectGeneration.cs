using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Contract
{
    public class ContractLogicProjectGeneration : IProjectGeneration
    {
        private static string DomainFolder = ".Contract/Logic/Model/{Domain}";

        private static string TemplateFolder = Folder.Executable + @"\Projects\Contract.Logic\Templates";

        private static string ILogicTemplateFileName = Path.Combine(TemplateFolder, "ILogicTemplate.txt");
        private static string ILogicDtoTemplateFileName = Path.Combine(TemplateFolder, "ILogicDtoTemplate.txt");
        private static string ILogicDtoDetailTemplateFileName = Path.Combine(TemplateFolder, "ILogicDtoDetailTemplate.txt");
        private static string ILogicCreateDtoTemplateFileName = Path.Combine(TemplateFolder, "ILogicCreateDtoTemplate.txt");
        private static string ILogicUpdateDtoTemplateFileName = Path.Combine(TemplateFolder, "ILogicUpdateDtoTemplate.txt");

        private static string ILogicFileName = "IEntitiesLogic.cs";
        private static string ILogicDtoFileName = "IEntity.cs";
        private static string ILogicDtoDetailFileName = "IEntityDetail.cs";
        private static string ILogicCreateDtoFileName = "IEntityCreate.cs";
        private static string ILogicUpdateDtoFileName = "IEntityUpdate.cs";

        private EntityCoreAddition entityCoreAddition;
        private DtoAddition dtoAddition;
        private DtoPropertyAddition propertyAddition;
        private PathService pathService;

        public ContractLogicProjectGeneration(
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
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, ILogicTemplateFileName, ILogicFileName);

            this.dtoAddition.AddDto(options, DomainFolder, ILogicDtoTemplateFileName, ILogicDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicDtoDetailTemplateFileName, ILogicDtoDetailFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicCreateDtoTemplateFileName, ILogicCreateDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicUpdateDtoTemplateFileName, ILogicUpdateDtoFileName);

        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicDtoFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicDtoDetailFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicCreateDtoFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicUpdateDtoFileName, true);
        }
    }
}