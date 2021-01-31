using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.API
{
    public class ApiProjectGeneration : IProjectGeneration
    {
        private static string DomainFolder = ".API/Model/{Domain}";

        private static string TemplateFolder = Folder.Executable + @"\Projects\Api\Templates";
        private static string ApiControllerTemplateFileName = Path.Combine(TemplateFolder, "ApiControllerTemplate.txt");
        private static string ApiCreateDtoTemplateFileName = Path.Combine(TemplateFolder, "ApiCreateDto.txt");
        private static string ApiUpdateDtoTemplateFileName = Path.Combine(TemplateFolder, "ApiUpdateDto.txt");

        private static string ApiControllerFileName = "EntitiesController.cs";
        private static string ApiCreateDtoFileName = "EntityCreate.cs";
        private static string ApiUpdateDtoFileName = "EntityUpdate.cs";

        private EntityCoreAddition entityCoreAddition;
        private DtoAddition dtoAddition;
        private DtoPropertyAddition propertyAddition;
        private PathService pathService;

        public ApiProjectGeneration(
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
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, ApiControllerTemplateFileName, ApiControllerFileName);

            this.dtoAddition.AddDto(options, DomainFolder, ApiCreateDtoTemplateFileName, ApiCreateDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ApiUpdateDtoTemplateFileName, ApiUpdateDtoFileName);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ApiCreateDtoFileName);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ApiUpdateDtoFileName);

            // TODO: Add Annotations based on PropertyType/PropertyTypeExtra
        }
    }
}