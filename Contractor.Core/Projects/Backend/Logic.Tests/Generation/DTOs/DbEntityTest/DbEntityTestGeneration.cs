using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC_TESTS })]
    internal class DbEntityTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "DbEntityTestTemplate.txt");

        private static readonly string FileName = "DbEntityTest.cs";

        private readonly DbEntityTestMethodsAddition logicDbDtoTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public DbEntityTestGeneration(
            DbEntityTestMethodsAddition logicDbDtoTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.logicDbDtoTestMethodsAddition = logicDbDtoTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, LogicTestsProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDbDtoTestMethodsAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions guidPropertyOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(guidPropertyOptions);

            this.propertyAddition.AddPropertyToDTO(propertyAdditionOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDbDtoTestMethodsAddition.Edit(propertyAdditionOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}