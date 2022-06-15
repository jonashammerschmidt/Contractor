namespace Contractor.Core.Projects.Database.Persistence.InsertData.Dev
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class PersistenceInsertDataDevProjectFileGeneration : ClassGeneration
    {
        private static readonly string FileName = "dbo.Entities.csv";

        private readonly PersistenceInsertDataDevProjectFileEntityAddition entityAddition;

        public PersistenceInsertDataDevProjectFileGeneration(
            PersistenceInsertDataDevProjectFileEntityAddition entityAddition)
        {
            this.entityAddition = entityAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityAddition.Add(entity);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }
    }
}