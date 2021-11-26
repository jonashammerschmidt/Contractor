using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityUpdateTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityUpdateTestTemplate.txt");

        private static readonly string FileName = "EntityUpdateTest.cs";

        private readonly EntityUpdateTestMethodsAddition logicDtoUpdateTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EntityUpdateTestGeneration(
            EntityUpdateTestMethodsAddition logicDtoUpdateTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.logicDtoUpdateTestMethodsAddition = logicDtoUpdateTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, LogicTestsProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDtoUpdateTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions guidPropertyOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(guidPropertyOptions, LogicTestsProjectGeneration.DtoFolder, FileName);

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(guidPropertyOptions);
            this.logicDtoUpdateTestMethodsAddition.Add(propertyAdditionOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}