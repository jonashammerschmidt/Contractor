using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC_TESTS })]
    internal class EntityCreateTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityCreateTestTemplate.txt");

        private static readonly string FileName = "EntityCreateTest.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityCreateTestMethodsAddition logicDtoCreateTestMethodsAddition;

        public EntityCreateTestGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition,
            EntityCreateTestMethodsAddition logicDtoCreateTestMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.logicDtoCreateTestMethodsAddition = logicDtoCreateTestMethodsAddition;
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
            this.logicDtoCreateTestMethodsAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions relationAdditionOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(relationAdditionOptions, LogicTestsProjectGeneration.DtoFolder, FileName);

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationAdditionOptions);
            this.logicDtoCreateTestMethodsAddition.Edit(propertyAdditionOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}