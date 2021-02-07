using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Projects.Persistence;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Logic
{
    public class LogicProjectGeneration : IProjectGeneration
    {
        private static readonly string ProjectFolder = ".Logic";
        private static readonly string DomainFolder = ".Logic/Model/{Domain}/{Entities}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\Logic\Templates";
        private static readonly string LogicTemplateFileName = Path.Combine(TemplateFolder, "LogicTemplate.txt");
        private static readonly string LogicDtoTemplateFileName = Path.Combine(TemplateFolder, "LogicDtoTemplate.txt");
        private static readonly string LogicDbDtoTemplateFileName = Path.Combine(TemplateFolder, "LogicDbDtoTemplate.txt");
        private static readonly string LogicDtoDetailTemplateFileName = Path.Combine(TemplateFolder, "LogicDtoDetailTemplate.txt");

        private static readonly string LogicFileName = "EntitiesCrudLogic.cs";
        private static readonly string LogicDbDtoFileName = "DbEntity.cs";
        private static readonly string LogicDtoFileName = "Entity.cs";
        private static readonly string LogicDtoDetailFileName = "EntityDetail.cs";

        private static readonly string LogicDependencyProviderFileName = "DependencyProvider.cs";

        private readonly DomainDependencyProvider domainDependencyProvider;
        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntityCoreDependencyProvider entityCoreDependencyProvider;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoDetailMethodsAddition dtoDetailMethodsAddition;
        private readonly DtoMethodsAddition dtoMethodsAddition;
        private readonly PathService pathService;

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

        public void AddDomain(DomainOptions options)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, LogicDependencyProviderFileName);
        }

        public void AddEntity(EntityOptions options)
        {
            this.pathService.AddEntityFolder(options, DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, LogicTemplateFileName, LogicFileName);

            this.pathService.AddDtoFolder(options, DomainFolder);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDtoTemplateFileName, LogicDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDbDtoTemplateFileName, LogicDbDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDtoDetailTemplateFileName, LogicDtoDetailFileName);

            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, LogicDependencyProviderFileName);
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