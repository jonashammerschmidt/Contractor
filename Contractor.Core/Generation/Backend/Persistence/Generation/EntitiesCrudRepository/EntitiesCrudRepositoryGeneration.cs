﻿using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE, ClassGenerationTag.BACKEND_PERSISTENCE_REPOSITORY })]
    internal class EntitiesCrudRepositoryGeneration : ClassGeneration
    {
        public static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "EntitiesCrudRepositoryTemplate.txt");

        public static readonly string FileName = "EntitiesCrudRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudRepositoryToRelationAddition entitiesCrudRepositoryToRelationAddition;
        private readonly EntitiesCrudRepositoryFromIncludeAddition dtoFromRepositoryIncludeAddition;
        private readonly EntitiesCrudRepositoryToIncludeAddition dtoToRepositoryIncludeAddition;
        private readonly EntitiesCrudRepositoryToOneToOneIncludeAddition entitiesCrudRepositoryToOneToOneIncludeAddition;

        public EntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudRepositoryToRelationAddition entitiesCrudRepositoryToRelationAddition,
            EntitiesCrudRepositoryFromIncludeAddition dtoFromRepositoryIncludeAddition,
            EntitiesCrudRepositoryToIncludeAddition dtoToRepositoryIncludeAddition,
            EntitiesCrudRepositoryToOneToOneIncludeAddition entitiesCrudRepositoryToOneToOneIncludeAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.entitiesCrudRepositoryToRelationAddition = entitiesCrudRepositoryToRelationAddition;
            this.dtoFromRepositoryIncludeAddition = dtoFromRepositoryIncludeAddition;
            this.dtoToRepositoryIncludeAddition = dtoToRepositoryIncludeAddition;
            this.entitiesCrudRepositoryToOneToOneIncludeAddition = entitiesCrudRepositoryToOneToOneIncludeAddition;
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
            if (relation.IsCreatedByPreProcessor)
            {
                return;
            }

            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");

            this.dtoFromRepositoryIncludeAddition.AddRelationSideToBackendFile(relationSideFrom, PersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "");

            this.dtoToRepositoryIncludeAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DomainFolder, FileName);
            this.entitiesCrudRepositoryToRelationAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            if (relation.IsCreatedByPreProcessor)
            {
                return;
            }

            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");

        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "");

            this.entitiesCrudRepositoryToOneToOneIncludeAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DomainFolder, FileName);
            this.entitiesCrudRepositoryToRelationAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}