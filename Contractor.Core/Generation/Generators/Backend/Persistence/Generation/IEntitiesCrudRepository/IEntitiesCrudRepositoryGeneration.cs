﻿using Contractor.Core.BaseClasses;
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
        private readonly IEntitiesCrudRepositoryCustomDtoInserter iEntitiesCrudRepositoryCustomDtoInserter;

        public IEntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            IEntitiesCrudRepositoryCustomDtoInserter iEntitiesCrudRepositoryCustomDtoInserter)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.iEntitiesCrudRepositoryCustomDtoInserter = iEntitiesCrudRepositoryCustomDtoInserter;
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

        public void AddCustomDto(CustomDto customDto)
        {
            this.iEntitiesCrudRepositoryCustomDtoInserter.Insert(customDto, PersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}