using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    internal class IEntitiesCrudRepositoryGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractPersistenceProjectGeneration.TemplateFolder, "IEntitiesCrudRepositoryTemplate.txt");

        private static readonly string FileName = "IEntitiesCrudRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly IEntitiesCrudRepositoryToOneToOneRelationAddition entitiesCrudRepositoryToOneToOneRelationAddition;

        public IEntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            IEntitiesCrudRepositoryToOneToOneRelationAddition entitiesCrudRepositoryToOneToOneRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.entitiesCrudRepositoryToOneToOneRelationAddition = entitiesCrudRepositoryToOneToOneRelationAddition;
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

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entitiesCrudRepositoryToOneToOneRelationAddition.Edit(options, ContractPersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}