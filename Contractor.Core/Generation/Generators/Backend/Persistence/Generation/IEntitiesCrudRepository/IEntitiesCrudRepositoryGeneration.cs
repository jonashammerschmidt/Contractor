using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;

namespace Contractor.Core.Generation.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE })]
    public class IEntitiesCrudRepositoryGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "IEntitiesCrudRepositoryTemplate.txt");

        private static readonly string FileName = Path.Combine("Interfaces", "IEntitiesCrudRepository.cs");

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly IEntitiesCrudRepositoryPurposeDtoInserter iEntitiesCrudRepositoryPurposeDtoInserter;

        public IEntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            IEntitiesCrudRepositoryPurposeDtoInserter iEntitiesCrudRepositoryPurposeDtoInserter)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.iEntitiesCrudRepositoryPurposeDtoInserter = iEntitiesCrudRepositoryPurposeDtoInserter;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackend(entity, PersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
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

        public void AddPurposeDto(PurposeDto purposeDto)
        {
            this.iEntitiesCrudRepositoryPurposeDtoInserter.Insert(purposeDto, PersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}