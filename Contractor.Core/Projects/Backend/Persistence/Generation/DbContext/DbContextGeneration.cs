using Contractor.Core.Options;

namespace Contractor.Core.Projects.Backend.Persistence
{
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

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dbContextEntityAddition.Add(options);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.dbContextPropertyAddition.Edit(options);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.dbContextRelationToAddition.Edit(options);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.dbContextRelationToOneToOneAddition.Edit(options);
        }
    }
}