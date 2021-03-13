using Contractor.Core.Options;

namespace Contractor.Core.Projects.Persistence.Tests
{
    internal class InMemoryDbContextGeneration : ClassGeneration
    {
        private readonly InMemoryDbContextEntityAddition inMemoryDbContextEntityAddition;

        public InMemoryDbContextGeneration(
            InMemoryDbContextEntityAddition inMemoryDbContextEntityAddition)
        {
            this.inMemoryDbContextEntityAddition = inMemoryDbContextEntityAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.inMemoryDbContextEntityAddition.Add(options);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}