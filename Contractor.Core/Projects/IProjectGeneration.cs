using Contractor.Core.Jobs;

namespace Contractor.Core.Template
{
    public interface IProjectGeneration
    {
        void AddDomain(IDomainAdditionOptions options);

        void AddEntity(IEntityAdditionOptions options);

        void AddProperty(IPropertyAdditionOptions options);

        void Add1ToNRelation(IRelationAdditionOptions options);
    }
}