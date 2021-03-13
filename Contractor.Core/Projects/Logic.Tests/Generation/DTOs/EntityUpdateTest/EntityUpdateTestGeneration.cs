using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
    internal class EntityUpdateTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityUpdateTestTemplate.txt");

        private static readonly string FileName = "EntityUpdateTest.cs";

        private readonly LogicDtoUpdateTestMethodsAddition logicDtoUpdateTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public EntityUpdateTestGeneration(
            LogicDtoUpdateTestMethodsAddition logicDtoUpdateTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.logicDtoUpdateTestMethodsAddition = logicDtoUpdateTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, LogicTestsProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoUpdateTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IPropertyAdditionOptions guidPropertyOptions = 
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.propertyAddition.AddPropertyToDTO(guidPropertyOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoUpdateTestMethodsAddition.Add(guidPropertyOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}