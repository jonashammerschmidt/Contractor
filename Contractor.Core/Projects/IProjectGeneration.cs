using Contractor.Core.Jobs;

namespace Contractor.Core.Template
{
    public interface IProjectGeneration
    {
        void AddDomain(DomainOptions options);

        void AddEntity(EntityOptions options);

        void AddProperty(PropertyOptions options);
    }
}