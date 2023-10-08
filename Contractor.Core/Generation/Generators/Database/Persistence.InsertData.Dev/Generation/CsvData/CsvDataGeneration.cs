using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Database.Persistence.InsertData.Dev
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class CsvDataGeneration : ClassGeneration
    {
        private static readonly string FileName = "dbo.Entities.csv";

        private readonly CsvDataEntityAddition csvDataEntityAddition;
        private readonly CsvDataPropertyAddition csvDataPropertyAddition;
        private readonly CsvDataRelationToAddition csvDataRelationToAddition;

        public CsvDataGeneration(
            CsvDataEntityAddition csvDataEntityAddition,
            CsvDataPropertyAddition CsvDataPropertyAddition,
            CsvDataRelationToAddition CsvDataRelationToAddition)
        {
            this.csvDataEntityAddition = csvDataEntityAddition;
            this.csvDataPropertyAddition = CsvDataPropertyAddition;
            this.csvDataRelationToAddition = CsvDataRelationToAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.csvDataEntityAddition.Add(entity, PersistenceInsertDataDevProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.csvDataPropertyAddition.Add(property, PersistenceInsertDataDevProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.csvDataRelationToAddition.AddRelationSideToDatabaseFile(relationSideTo, PersistenceInsertDataDevProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            this.Add1ToNRelationSideTo(new Relation1ToN(relation));
        }
    }
}