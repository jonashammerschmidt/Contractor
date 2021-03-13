using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
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

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, LogicTestsProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.logicDtoTestValuesAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            this.logicDtoTestValuesRelationAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}