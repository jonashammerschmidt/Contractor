using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Contract.Persistence
{
    internal class IEntitiesCrudRepositoryGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractPersistenceProjectGeneration.TemplateFolder, "IEntitiesCrudRepositoryTemplate.txt");

        private static readonly string FileName = "IEntitiesCrudRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;

        public IEntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, ContractPersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}