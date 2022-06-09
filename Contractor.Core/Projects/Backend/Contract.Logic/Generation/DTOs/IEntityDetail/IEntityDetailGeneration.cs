using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_LOGIC })]
    internal class IEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityDetailTemplate.txt");

        private static readonly string FileName = "IEntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IEntityDetailGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, ContractLogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, ContractLogicProjectGeneration.DtoFolder, FileName, true);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>");
            this.relationAddition.AddRelationToDTO(optionsFrom, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"I{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(optionsFrom, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}