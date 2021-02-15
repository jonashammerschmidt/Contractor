using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Projects;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class LogicProjectGeneration : IProjectGeneration
    {
        private static readonly string ProjectFolder = "Logic";
        private static readonly string DomainFolder = "Logic\\Modules\\{Domain}\\{Entities}";

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
        private readonly DtoDetailFromMethodsAddition dtoDetailFromMethodsAddition;
        private readonly DtoDetailToMethodsAddition dtoDetailToMethodsAddition;
        private readonly LogicRelationAddition logicRelationAddition;
        private readonly PathService pathService;

        public LogicProjectGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreAddition entityCoreAddition,
            EntityCoreDependencyProvider entityCoreDependencyProvider,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoMethodsAddition dtoMethodsAddition,
            DtoDetailMethodsAddition dtoDetailMethodsAddition,
            DtoDetailFromMethodsAddition dtoDetailMethodsFromListAddition,
            DtoDetailToMethodsAddition dtoDetailToMethodsAddition,
            LogicRelationAddition logicRelationAddition,
            PathService pathService)
        {
            this.domainDependencyProvider = domainDependencyProvider;
            this.entityCoreAddition = entityCoreAddition;
            this.entityCoreDependencyProvider = entityCoreDependencyProvider;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.dtoMethodsAddition = dtoMethodsAddition;
            this.dtoDetailMethodsAddition = dtoDetailMethodsAddition;
            this.dtoDetailFromMethodsAddition = dtoDetailMethodsFromListAddition;
            this.dtoDetailToMethodsAddition = dtoDetailToMethodsAddition;
            this.logicRelationAddition = logicRelationAddition;
            this.pathService = pathService;
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, LogicDependencyProviderFileName);
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            this.pathService.AddEntityFolder(options, DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, DomainFolder, LogicTemplateFileName, LogicFileName);

            this.pathService.AddDtoFolder(options, DomainFolder);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDtoTemplateFileName, LogicDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDbDtoTemplateFileName, LogicDbDtoFileName);
            this.dtoAddition.AddDto(options, DomainFolder, LogicDtoDetailTemplateFileName, LogicDtoDetailFileName);

            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, LogicDependencyProviderFileName);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, LogicDtoFileName);
            this.dtoMethodsAddition.Add(options, DomainFolder, LogicDtoFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, LogicDbDtoFileName);

            this.propertyAddition.AddPropertyToDTO(options, DomainFolder, LogicDtoDetailFileName);
            this.dtoDetailMethodsAddition.Add(options, DomainFolder, LogicDtoDetailFileName);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>", $"{options.EntityNamePluralTo}"),
                DomainFolder, LogicDtoDetailFileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            this.dtoDetailFromMethodsAddition.Add(options, DomainFolder, LogicDtoDetailFileName,
                $"{options.ProjectName}.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, LogicDtoFileName);
            this.dtoMethodsAddition.Add(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, LogicDtoFileName);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, LogicDbDtoFileName);

            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}", options.EntityNameFrom),
                DomainFolder, LogicDtoDetailFileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dtoDetailToMethodsAddition.Add(options, DomainFolder, LogicDtoDetailFileName);

            this.logicRelationAddition.Add(options, DomainFolder, LogicFileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}