using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityDetailTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityDetailTestTemplate.txt");

        private static readonly string FileName = "EntityDetailTest.cs";

        private readonly EntityDetailTestFromAssertAddition logicDtoDetailTestFromAssertAddition;
        private readonly EntityDetailTestMethodsAddition logicDtoDetailTestMethodsAddition;
        private readonly EntityDetailTestToAssertAddition logicDtoDetailTestToAssertAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EntityDetailTestGeneration(
            EntityDetailTestFromAssertAddition logicDtoDetailTestFromAssertAddition,
            EntityDetailTestMethodsAddition logicDtoDetailTestMethodsAddition,
            EntityDetailTestToAssertAddition logicDtoDetailTestToAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.logicDtoDetailTestFromAssertAddition = logicDtoDetailTestFromAssertAddition;
            this.logicDtoDetailTestToAssertAddition = logicDtoDetailTestToAssertAddition;
            this.logicDtoDetailTestMethodsAddition = logicDtoDetailTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
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
            IRelationSideAdditionOptions dtoFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>");
            this.relationAddition.AddRelationToDTO(dtoFromOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoDetailTestFromAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            IRelationSideAdditionOptions dtoToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dtoToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoDetailTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}