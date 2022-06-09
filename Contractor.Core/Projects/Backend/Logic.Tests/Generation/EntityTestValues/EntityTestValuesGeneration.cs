using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC_TESTS })]
    internal class EntityTestValuesGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityTestValuesTemplate.txt");

        private static readonly string FileName = "EntityTestValues.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntityTestValuesAddition logicDtoTestValuesAddition;
        private readonly EntityTestValuesRelationAddition logicDtoTestValuesRelationAddition;

        public EntityTestValuesGeneration(
            EntityCoreAddition entityCoreAddition,
            EntityTestValuesAddition logicDtoTestValuesAddition,
            EntityTestValuesRelationAddition logicDtoTestValuesRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicDtoTestValuesAddition = logicDtoTestValuesAddition;
            this.logicDtoTestValuesRelationAddition = logicDtoTestValuesRelationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityCore(entity, LogicTestsProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.logicDtoTestValuesAddition.Edit(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            this.logicDtoTestValuesRelationAddition.Edit(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // To
            this.logicDtoTestValuesRelationAddition.Edit(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}