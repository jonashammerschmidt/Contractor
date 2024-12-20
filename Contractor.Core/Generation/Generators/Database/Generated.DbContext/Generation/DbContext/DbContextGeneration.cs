﻿using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Database.Generated.DbContext
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    public class DbContextGeneration : ClassGeneration
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
            this.dbContextPropertyAddition.AddPropertyToDatabaseFile(property, "Generated.DbContext", $"DbContextNameRaw.cs");
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "Ef", "Dto");
            this.dbContextRelationToAddition.AddRelationSideToDatabaseFile(relationSideTo, "Generated.DbContext", "DbContextNameRaw.cs");
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "Ef", "Dto");
            this.dbContextRelationToOneToOneAddition.AddRelationSideToDatabaseFile(relationSideTo, "Generated.DbContext", "DbContextNameRaw.cs");
        }
    }
}