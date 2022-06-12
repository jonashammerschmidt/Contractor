﻿namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class DbContextGeneration : ClassGeneration
    {
        private readonly DbContextRelationToAddition dbContextRelationToAddition;
        private readonly DbContextRelationToOneToOneAddition dbContextRelationToOneToOneAddition;

        public DbContextGeneration(
            DbContextRelationToAddition dbContextRelationToAddition,
            DbContextRelationToOneToOneAddition dbContextRelationToOneToOneAddition)
        {
            this.dbContextRelationToAddition = dbContextRelationToAddition;
            this.dbContextRelationToOneToOneAddition = dbContextRelationToOneToOneAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "Ef", "");
            this.dbContextRelationToAddition.Edit(relationSideTo);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "Ef", "");
            this.dbContextRelationToOneToOneAddition.Edit(relationSideTo);
        }
    }
}