using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityListItemTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityListItemTestTemplate.txt");

        private static readonly string FileName = "EntityListItemTest.cs";

        private readonly EntityListItemTestMethodsAddition logicDtoListItemTestMethodsAddition;
        private readonly EntityListItemTestToAssertAddition logicDtoListItemTestToAssertAddition;
        private readonly EntityListItemTestFromOneToOneAssertAddition entityListItemTestFromOneToOneAssertAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EntityListItemTestGeneration(
            EntityListItemTestMethodsAddition logicDtoListItemTestMethodsAddition,
            EntityListItemTestToAssertAddition logicDtoListItemTestToAssertAddition,
            EntityListItemTestFromOneToOneAssertAddition entityListItemTestFromOneToOneAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.logicDtoListItemTestToAssertAddition = logicDtoListItemTestToAssertAddition;
            this.logicDtoListItemTestMethodsAddition = logicDtoListItemTestMethodsAddition;
            this.entityListItemTestFromOneToOneAssertAddition = entityListItemTestFromOneToOneAssertAddition;
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
            this.logicDtoListItemTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions dtoToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dtoToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoListItemTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions dtoFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"I{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(dtoFromOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.entityListItemTestFromOneToOneAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            IRelationSideAdditionOptions dtoToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dtoToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoListItemTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
} 