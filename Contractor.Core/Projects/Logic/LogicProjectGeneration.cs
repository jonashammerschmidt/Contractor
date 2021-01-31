using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Projects.Persistence;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Logic
{
    public class LogicProjectGeneration : IProjectGeneration
    {
        private static string ProjectFolder = ".Logic";
        private static string DomainFolder = ".Logic/Model/{Domain}";

        private static string TemplateFolder = Folder.Executable + @"\Projects\Logic\Templates";
        private static string LogicTemplateFileName = Path.Combine(TemplateFolder, "LogicTemplate.txt");
        private static string LogicDtoTemplateFileName = Path.Combine(TemplateFolder, "LogicDtoTemplate.txt");
        private static string LogicDbDtoTemplateFileName = Path.Combine(TemplateFolder, "LogicDbDtoTemplate.txt");
        private static string LogicDtoDetailTemplateFileName = Path.Combine(TemplateFolder, "LogicDtoDetailTemplate.txt");

        private static string LogicFileName = "EntitiesLogic.cs";
        private static string LogicDbDtoFileName = "DbEntity.cs";
        private static string LogicDtoFileName = "Entity.cs";
        private static string LogicDtoDetailFileName = "EntityDetail.cs";

        private static string LogicDependencyProviderFileName = "DependencyProvider.cs";

        private DomainDependencyProvider domainDependencyProvider;
        private EntityCoreAddition entityCoreAddition;
        private EntityCoreDependencyProvider entityCoreDependencyProvider;
        private DtoAddition dtoAddition;
        private DtoPropertyAddition propertyAddition;
        private DtoDetailMethodsAddition dtoDetailMethodsAddition;
        private DtoMethodsAddition dtoMethodsAddition;
        private PathService pathService;

        public LogicProjectGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreAddition entityCoreAddition,
            EntityCoreDependencyProvider entityCoreDependencyProvider,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoMethodsAddition dtoMethodsAddition,
            DtoDetailMethodsAddition dtoDetailMethodsAddition,
            PathService pathService)
        {
            this.domainDependencyProvider = domainDependencyProvider;
            this.entityCoreAddition = entityCoreAddition;
            this.entityCoreDependencyProvider = entityCoreDependencyProvider;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.dtoMethodsAddition = dtoMethodsAddition;
            this.dtoDetailMethodsAddition = dtoDetailMethodsAddition;
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

            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, LogicDependencyProviderFileName);
        }

        public void AddEntity(EntityOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, LogicTemplateFileName, LogicFileName);

            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, LogicDependencyProviderFileName);

            this.dtoAddition.AddDto(options, DomainFolder, LogicDtoTemplateFileName, LogicDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDbDtoTemplateFileName, LogicDbDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDtoDetailTemplateFileName, LogicDtoDetailFileName);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, LogicDtoFileName);
            this.dtoMethodsAddition.Add(options, DomainFolder, LogicDtoFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, LogicDbDtoFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, LogicDtoDetailFileName);
            this.dtoDetailMethodsAddition.Add(options, DomainFolder, LogicDtoDetailFileName);
        }
    }
}