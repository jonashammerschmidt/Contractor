using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_PERSISTENCE })]
    internal class EntitiesReadRepositoryDefaultGeneration : ClassGeneration
    {
        public static readonly string TemplatePath =
            Path.Combine(GeneratedPersistenceProjectGeneration.TemplateFolder, "EntitiesReadRepositoryDefaultTemplate.txt");

        public static readonly string FileName = "EntitiesReadRepositoryDefault.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesReadRepositoryDefaultFromIncludeAddition entitiesReadRepositoryDefaultFromIncludeAddition;
        private readonly EntitiesReadRepositoryDefaultToIncludeAddition entitiesReadRepositoryDefaultToIncludeAddition;
        private readonly EntitiesReadRepositoryDefaultFromOneToOneIncludeAddition entitiesReadRepositoryDefaultFromOneToOneIncludeAddition;
        private readonly EntitiesReadRepositoryDefaultToOneToOneIncludeAddition entitiesReadRepositoryDefaultToOneToOneIncludeAddition;

        public EntitiesReadRepositoryDefaultGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesReadRepositoryDefaultFromIncludeAddition entitiesReadRepositoryDefaultFromIncludeAddition,
            EntitiesReadRepositoryDefaultToIncludeAddition entitiesReadRepositoryDefaultToIncludeAddition,
            EntitiesReadRepositoryDefaultFromOneToOneIncludeAddition entitiesReadRepositoryDefaultFromOneToOneIncludeAddition,
            EntitiesReadRepositoryDefaultToOneToOneIncludeAddition entitiesReadRepositoryDefaultToOneToOneIncludeAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.entitiesReadRepositoryDefaultFromIncludeAddition = entitiesReadRepositoryDefaultFromIncludeAddition;
            this.entitiesReadRepositoryDefaultToIncludeAddition = entitiesReadRepositoryDefaultToIncludeAddition;
            this.entitiesReadRepositoryDefaultFromOneToOneIncludeAddition = entitiesReadRepositoryDefaultFromOneToOneIncludeAddition;
            this.entitiesReadRepositoryDefaultToOneToOneIncludeAddition = entitiesReadRepositoryDefaultToOneToOneIncludeAddition;
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

            this.entitiesReadRepositoryDefaultFromIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "");

            this.entitiesReadRepositoryDefaultToIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            if (relation.IsCreatedByPreProcessor)
            {
                return;
            }

            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");

            this.entitiesReadRepositoryDefaultFromOneToOneIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideFrom, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "");

            this.entitiesReadRepositoryDefaultToOneToOneIncludeAddition.AddRelationSideToBackendGeneratedFile(relationSideTo, GeneratedPersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}