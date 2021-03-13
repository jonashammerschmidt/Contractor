using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
    internal class EntityDetailTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityDetailTestTemplate.txt");

        private static readonly string FileName = "EntityDetailTest.cs";

        private readonly LogicDtoDetailTestFromAssertAddition logicDtoDetailTestFromAssertAddition;
        private readonly LogicDtoDetailTestMethodsAddition logicDtoDetailTestMethodsAddition;
        private readonly LogicDtoDetailTestToAssertAddition logicDtoDetailTestToAssertAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public EntityDetailTestGeneration(
            LogicDtoDetailTestFromAssertAddition logicDtoDetailTestFromAssertAddition,
            LogicDtoDetailTestMethodsAddition logicDtoDetailTestMethodsAddition,
            LogicDtoDetailTestToAssertAddition logicDtoDetailTestToAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.logicDtoDetailTestFromAssertAddition = logicDtoDetailTestFromAssertAddition;
            this.logicDtoDetailTestToAssertAddition = logicDtoDetailTestToAssertAddition;
            this.logicDtoDetailTestMethodsAddition = logicDtoDetailTestMethodsAddition;
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
            this.logicDtoDetailTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IPropertyAdditionOptions dtoFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");
            this.propertyAddition.AddPropertyToDTO(dtoFromOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoDetailTestFromAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            IPropertyAdditionOptions dtoToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}", $"{options.EntityNameFrom}");
            this.propertyAddition.AddPropertyToDTO(dtoToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoDetailTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}