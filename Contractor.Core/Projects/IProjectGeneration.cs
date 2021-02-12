using Contractor.Core.Options;

namespace Contractor.Core.Projects
{
    internal interface IProjectGeneration
    {
        void AddDomain(IDomainAdditionOptions options);

        void AddEntity(IEntityAdditionOptions options);

        void AddProperty(IPropertyAdditionOptions options);

        void Add1ToNRelation(IRelationAdditionOptions options);
    }
}