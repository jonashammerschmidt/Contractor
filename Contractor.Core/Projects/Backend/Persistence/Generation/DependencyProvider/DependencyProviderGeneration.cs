using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DependencyProviderGeneration : ClassGeneration
    {
        private static readonly string ProjectFolder = "Persistence";

        private static readonly string FileName = "DependencyProvider.cs";

        private readonly DomainDependencyProvider domainDependencyProvider;
        private readonly EntityCoreDependencyProvider entityCoreDependencyProvider;

        public DependencyProviderGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreDependencyProvider entityCoreDependencyProvider)
        {
            this.domainDependencyProvider = domainDependencyProvider;
            this.entityCoreDependencyProvider = entityCoreDependencyProvider;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, FileName);
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, ProjectFolder, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}