using Contractor.Core.Options;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class DbContextGeneration : ClassGeneration
    {
        private readonly DbContextEntityAddition dbContextEntityAddition;
        private readonly DbContextPropertyAddition dbContextPropertyAddition;
        private readonly DbContextRelationToAddition dbContextRelationToAddition;
        private readonly DbContextRelationToOneToOneAddition dbContextRelationToOneToOneAddition;

        public DbContextGeneration(
            DbContextEntityAddition dbContextEntityAddition,
            DbContextPropertyAddition dbContextPropertyAddition,
            DbContextRelationToAddition dbContextRelationToAddition,
            DbContextRelationToOneToOneAddition dbContextRelationToOneToOneAddition)
        {
            this.dbContextEntityAddition = dbContextEntityAddition;
            this.dbContextPropertyAddition = dbContextPropertyAddition;
            this.dbContextRelationToAddition = dbContextRelationToAddition;
            this.dbContextRelationToOneToOneAddition = dbContextRelationToOneToOneAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dbContextEntityAddition.Add(entity);
        }

        protected override void AddProperty(Property property)
        {
            this.dbContextPropertyAddition.Edit(property);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.dbContextRelationToAddition.Edit(relationSideTo);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.dbContextRelationToOneToOneAddition.Edit(relationSideTo);
        }
    }
}