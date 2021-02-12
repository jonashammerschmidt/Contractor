using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class ApiProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = ".API\\Model\\{Domain}\\{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Api\Templates";
        private static readonly string ApiControllerTemplateFileName = Path.Combine(TemplateFolder, "ApiControllerTemplate.txt");
        private static readonly string ApiCreateDtoTemplateFileName = Path.Combine(TemplateFolder, "ApiCreateDto.txt");
        private static readonly string ApiUpdateDtoTemplateFileName = Path.Combine(TemplateFolder, "ApiUpdateDto.txt");

        private static readonly string ApiControllerFileName = "EntitiesCrudController.cs";
        private static readonly string ApiCreateDtoFileName = "EntityCreate.cs";
        private static readonly string ApiUpdateDtoFileName = "EntityUpdate.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

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

        public void AddDomain(IDomainAdditionOptions options)
        {
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            this.pathService.AddEntityFolder(options, DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, ApiControllerTemplateFileName, ApiControllerFileName);

            this.pathService.AddDtoFolder(options, DomainFolder);
            this.dtoAddition.AddDto(options, DomainFolder, ApiCreateDtoTemplateFileName, ApiCreateDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ApiUpdateDtoTemplateFileName, ApiUpdateDtoFileName);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ApiCreateDtoFileName);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ApiUpdateDtoFileName);

            // TODO: Add Annotations based on PropertyType/PropertyTypeExtra
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, ApiCreateDtoFileName);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, ApiUpdateDtoFileName);
        }
    }
}