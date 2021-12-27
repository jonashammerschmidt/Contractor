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
        public UsingStatementAddition usingStatementAddition;

        public EntitiesCrudLogicTestsGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicTestsRelationAddition logicTestsRelationAddition,
            EntitiesCrudLogicTestsToOneToOneRelationAddition entitiesCrudLogicTestsToOneToOneRelationAddition,
            UsingStatementAddition usingStatementAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicTestsRelationAddition = logicTestsRelationAddition;
            this.entitiesCrudLogicTestsToOneToOneRelationAddition = entitiesCrudLogicTestsToOneToOneRelationAddition;
            this.usingStatementAddition = usingStatementAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, LogicTestsProjectGeneration.DomainFolder, TemplatePath, FileName);
            this.usingStatementAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName, "Microsoft.EntityFrameworkCore");
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.logicTestsRelationAddition.Edit(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entitiesCrudLogicTestsToOneToOneRelationAddition.Edit(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}