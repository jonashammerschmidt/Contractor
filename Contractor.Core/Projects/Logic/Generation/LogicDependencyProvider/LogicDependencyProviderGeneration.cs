using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Logic
{
    internal class LogicDependencyProviderGeneration : ClassGeneration
    {
        private static readonly string FileName = "DependencyProvider.cs";

        private readonly DomainDependencyProvider domainDependencyProvider;
        private readonly EntityCoreDependencyProvider entityCoreDependencyProvider;

        public LogicDependencyProviderGeneration(
            DomainDependencyProvider domainDependencyProvider,
            EntityCoreDependencyProvider entityCoreDependencyProvider)
        {
            this.domainDependencyProvider = domainDependencyProvider;
            this.entityCoreDependencyProvider = entityCoreDependencyProvider;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(options, LogicProjectGeneration.ProjectFolder, FileName);
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreDependencyProvider.UpdateDependencyProvider(options, LogicProjectGeneration.ProjectFolder, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}