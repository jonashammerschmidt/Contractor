using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE })]
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

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityCore(entity, ContractPersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.entitiesCrudRepositoryToOneToOneRelationAddition.Edit(relationSide, ContractPersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}