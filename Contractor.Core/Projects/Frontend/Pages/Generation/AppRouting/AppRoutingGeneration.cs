using Contractor.Core.Options;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class AppRoutingGeneration : ClassGeneration
    {
        private readonly AppRoutingDomainAddition appRoutingDomainAddition;

        public AppRoutingGeneration(AppRoutingDomainAddition appRoutingDomainAddition)
        {
            this.appRoutingDomainAddition = appRoutingDomainAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
            this.appRoutingDomainAddition.Add(options);
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