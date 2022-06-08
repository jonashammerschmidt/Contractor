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

        protected override void AddEntity(IEntityAdditionOptions options)
        {
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