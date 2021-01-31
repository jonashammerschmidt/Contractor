using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;

namespace Contractor.Core.Template
{
    public interface IProjectGeneration
    {
        void ClearDomain(DomainOptions options);

        void AddDomain(DomainOptions options);

        void AddEntity(EntityOptions options);

        void AddProperty(PropertyOptions options);
    }
}