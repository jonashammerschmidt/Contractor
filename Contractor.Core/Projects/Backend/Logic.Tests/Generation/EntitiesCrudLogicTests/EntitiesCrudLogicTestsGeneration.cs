using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntitiesCrudLogicTestsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntitiesCrudLogicTestsTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogicTests.cs";

        public EntityCoreAddition entityCoreAddition;
        public EntitiesCrudLogicTestsRelationAddition logicTestsRelationAddition;
        public EntitiesCrudLogicTestsToOneToOneRelationAddition entitiesCrudLogicTestsToOneToOneRelationAddition;

        public EntitiesCrudLogicTestsGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicTestsRelationAddition logicTestsRelationAddition,
            EntitiesCrudLogicTestsToOneToOneRelationAddition entitiesCrudLogicTestsToOneToOneRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicTestsRelationAddition = logicTestsRelationAddition;
            this.entitiesCrudLogicTestsToOneToOneRelationAddition = entitiesCrudLogicTestsToOneToOneRelationAddition;
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

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entitiesCrudLogicTestsToOneToOneRelationAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}