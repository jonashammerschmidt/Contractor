using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class ContractLogicProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = ".Contract\\Logic\\Model\\{Domain}\\{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Contract.Logic\Templates";

        private static readonly string ILogicTemplateFileName = Path.Combine(TemplateFolder, "ILogicTemplate.txt");
        private static readonly string ILogicDtoTemplateFileName = Path.Combine(TemplateFolder, "ILogicDtoTemplate.txt");
        private static readonly string ILogicDtoDetailTemplateFileName = Path.Combine(TemplateFolder, "ILogicDtoDetailTemplate.txt");
        private static readonly string ILogicCreateDtoTemplateFileName = Path.Combine(TemplateFolder, "ILogicCreateDtoTemplate.txt");
        private static readonly string ILogicUpdateDtoTemplateFileName = Path.Combine(TemplateFolder, "ILogicUpdateDtoTemplate.txt");

        private static readonly string ILogicFileName = "IEntitiesCrudLogic.cs";
        private static readonly string ILogicDtoFileName = "IEntity.cs";
        private static readonly string ILogicDtoDetailFileName = "IEntityDetail.cs";
        private static readonly string ILogicCreateDtoFileName = "IEntityCreate.cs";
        private static readonly string ILogicUpdateDtoFileName = "IEntityUpdate.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly PathService pathService;

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

        public void AddDomain(IDomainAdditionOptions options)
        {
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            this.pathService.AddEntityFolder(options, DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, ILogicTemplateFileName, ILogicFileName);

            this.pathService.AddDtoFolder(options, DomainFolder);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicDtoTemplateFileName, ILogicDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicDtoDetailTemplateFileName, ILogicDtoDetailFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicCreateDtoTemplateFileName, ILogicCreateDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, ILogicUpdateDtoTemplateFileName, ILogicUpdateDtoFileName);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicDtoFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicDtoDetailFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicCreateDtoFileName, true);
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, ILogicUpdateDtoFileName, true);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>", $"{options.EntityNamePluralTo}"),
                DomainFolder, ILogicDtoDetailFileName, true,
                $"{options.ProjectName}.Contract.Logic.Model.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, ILogicDtoFileName, true);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}", options.EntityNameFrom),
                DomainFolder, ILogicDtoDetailFileName, true,
                $"{options.ProjectName}.Contract.Logic.Model.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, ILogicCreateDtoFileName, true);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, ILogicUpdateDtoFileName, true);
        }
    }
}