using Contractor.Core.Options;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class AppRoutingGeneration : ClassGeneration
    {
        private readonly AppRoutingDomainAddition appRoutingDomainAddition;

        public AppRoutingGeneration(AppRoutingDomainAddition appRoutingDomainAddition)
        {
            this.appRoutingDomainAddition = appRoutingDomainAddition;
        }

        protected override void AddModuleActions(Module module)
        {
            this.appRoutingDomainAddition.Add(module);
        }

        protected override void AddEntity(Entity entity)
        {
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