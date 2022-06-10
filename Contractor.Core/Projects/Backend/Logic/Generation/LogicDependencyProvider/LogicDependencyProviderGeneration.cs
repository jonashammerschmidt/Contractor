using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
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

        protected override void AddModuleActions(Module module)
        {
            this.domainDependencyProvider.UpdateDependencyProvider(module, LogicProjectGeneration.ProjectFolder, FileName);
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreDependencyProvider.UpdateDependencyProvider(entity, LogicProjectGeneration.ProjectFolder, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
        }
    }
}