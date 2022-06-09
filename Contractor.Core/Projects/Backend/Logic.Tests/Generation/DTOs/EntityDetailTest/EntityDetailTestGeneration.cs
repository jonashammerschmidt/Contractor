﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC_TESTS })]
    internal class EntityDetailTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityDetailTestTemplate.txt");

        private static readonly string FileName = "EntityDetailTest.cs";

        private readonly EntityDetailTestFromAssertAddition logicDtoDetailTestFromAssertAddition;
        private readonly EntityDetailTestFromOneToOneAssertAddition entityDetailTestFromOneToOneAssertAddition;
        private readonly EntityDetailTestMethodsAddition logicDtoDetailTestMethodsAddition;
        private readonly EntityDetailTestToAssertAddition logicDtoDetailTestToAssertAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EntityDetailTestGeneration(
            EntityDetailTestFromAssertAddition logicDtoDetailTestFromAssertAddition,
            EntityDetailTestFromOneToOneAssertAddition entityDetailTestFromOneToOneAssertAddition,
            EntityDetailTestMethodsAddition logicDtoDetailTestMethodsAddition,
            EntityDetailTestToAssertAddition logicDtoDetailTestToAssertAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.logicDtoDetailTestFromAssertAddition = logicDtoDetailTestFromAssertAddition;
            this.entityDetailTestFromOneToOneAssertAddition = entityDetailTestFromOneToOneAssertAddition;
            this.logicDtoDetailTestToAssertAddition = logicDtoDetailTestToAssertAddition;
            this.logicDtoDetailTestMethodsAddition = logicDtoDetailTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
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
            this.logicDtoDetailTestMethodsAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions dtoFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>");
            this.relationAddition.AddRelationToDTO(dtoFromOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDtoDetailTestFromAssertAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);

            // To
            IRelationSideAdditionOptions dtoToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dtoToOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDtoDetailTestToAssertAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions dtoFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"I{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(dtoFromOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.entityDetailTestFromOneToOneAssertAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);

            // To
            IRelationSideAdditionOptions dtoToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"I{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dtoToOptions, LogicTestsProjectGeneration.DtoFolder, FileName);
            this.logicDtoDetailTestToAssertAddition.Edit(options, LogicTestsProjectGeneration.DtoFolder, FileName);
        }
    }
}