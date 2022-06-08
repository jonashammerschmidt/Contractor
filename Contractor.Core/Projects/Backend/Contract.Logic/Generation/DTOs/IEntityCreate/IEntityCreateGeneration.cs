using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_LOGIC })]
    internal class IEntityCreateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityCreateTemplate.txt");

        private static readonly string FileName = "IEntityCreate.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IEntityCreateGeneration(
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

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, ContractLogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, ContractLogicProjectGeneration.DtoFolder, FileName, true);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.relationAddition.AddRelationToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid"),
                ContractLogicProjectGeneration.DtoFolder, FileName, true);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}