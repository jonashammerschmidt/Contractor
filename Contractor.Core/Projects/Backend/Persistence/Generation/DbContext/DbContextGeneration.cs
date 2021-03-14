using Contractor.Core.Options;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbContextGeneration : ClassGeneration
    {
        private readonly DbContextEntityAddition dbContextEntityAddition;
        private readonly DbContextPropertyAddition dbContextPropertyAddition;
        private readonly DbContextRelationToAddition dbContextRelationToAddition;

        public DbContextGeneration(
            DbContextEntityAddition dbContextEntityAddition,
            DbContextPropertyAddition dbContextPropertyAddition,
            DbContextRelationToAddition dbContextRelationToAddition)
        {
            this.dbContextEntityAddition = dbContextEntityAddition;
            this.dbContextPropertyAddition = dbContextPropertyAddition;
            this.dbContextRelationToAddition = dbContextRelationToAddition;
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
            this.dbContextPropertyAddition.Add(options);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.dbContextRelationToAddition.Add(options);
        }
    }
}