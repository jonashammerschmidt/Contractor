using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC_TESTS })]
    internal class EntityTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityTestTemplate.txt");

        private static readonly string FileName = "EntityTest.cs";

        private readonly EntityTestMethodsAddition logicDtoTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EntityTestGeneration(
            EntityTestMethodsAddition logicDtoTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.logicDtoTestMethodsAddition = logicDtoTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, LogicTestsProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDtoTestMethodsAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions guidPropertyOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(guidPropertyOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
            
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(guidPropertyOptions);
            this.logicDtoTestMethodsAddition.Edit(propertyAdditionOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}