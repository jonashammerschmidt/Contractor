using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    public class AppRoutingGeneration : ClassGeneration
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

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }
    }
}