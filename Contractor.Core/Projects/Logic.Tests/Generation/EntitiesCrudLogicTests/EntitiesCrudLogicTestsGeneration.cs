using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
    internal class EntitiesCrudLogicTestsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntitiesCrudLogicTestsTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogicTests.cs";

        public EntityCoreAddition entityCoreAddition;
        public EntitiesCrudLogicTestsRelationAddition logicTestsRelationAddition;

        public EntitiesCrudLogicTestsGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicTestsRelationAddition logicTestsRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicTestsRelationAddition = logicTestsRelationAddition;
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
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.logicTestsRelationAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}