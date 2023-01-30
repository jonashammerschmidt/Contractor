using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_PERSISTENCE })]
    internal class EntitiesDefaultReadRepositoryGeneration : ClassGeneration
    {
        public static readonly string TemplatePath =
            Path.Combine(GeneratedPersistenceProjectGeneration.TemplateFolder, "EntitiesDefaultReadRepositoryTemplate.txt");

        public static readonly string FileName = "EntitiesDefaultReadRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesDefaultReadRepositoryFromIncludeAddition entitiesDefaultReadRepositoryFromIncludeAddition;
        private readonly EntitiesDefaultReadRepositoryToIncludeAddition entitiesDefaultReadRepositoryToIncludeAddition;
        private readonly EntitiesDefaultReadRepositoryFromOneToOneIncludeAddition entitiesDefaultReadRepositoryFromOneToOneIncludeAddition;
        private readonly EntitiesDefaultReadRepositoryToOneToOneIncludeAddition entitiesDefaultReadRepositoryToOneToOneIncludeAddition;

        public EntitiesDefaultReadRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesDefaultReadRepositoryFromIncludeAddition entitiesDefaultReadRepositoryFromIncludeAddition,
            EntitiesDefaultReadRepositoryToIncludeAddition entitiesDefaultReadRepositoryToIncludeAddition,
            EntitiesDefaultReadRepositoryFromOneToOneIncludeAddition entitiesDefaultReadRepositoryFromOneToOneIncludeAddition,
            EntitiesDefaultReadRepositoryToOneToOneIncludeAddition entitiesDefaultReadRepositoryToOneToOneIncludeAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.entitiesDefaultReadRepositoryFromIncludeAddition = entitiesDefaultReadRepositoryFromIncludeAddition;
            this.entitiesDefaultReadRepositoryToIncludeAddition = entitiesDefaultReadRepositoryToIncludeAddition;
            this.entitiesDefaultReadRepositoryFromOneToOneIncludeAddition = entitiesDefaultReadRepositoryFromOneToOneIncludeAddition;
            this.entitiesDefaultReadRepositoryToOneToOneIncludeAddition = entitiesDefaultReadRepositoryToOneToOneIncludeAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedPersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            if (relation.IsCreatedByPreProcessor)
            {
                return;
            }

            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");

            this.entitiesDefaultReadRepositoryFromIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "");

            this.entitiesDefaultReadRepositoryToIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            if (relation.IsCreatedByPreProcessor)
            {
                return;
            }

            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");

            this.entitiesDefaultReadRepositoryFromOneToOneIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "");

            this.entitiesDefaultReadRepositoryToOneToOneIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}