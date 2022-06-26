﻿using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Api
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_API })]
    internal class EntityUpdateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ApiProjectGeneration.TemplateFolder, "EntityUpdateTemplate.txt");

        private static readonly string FileName = "EntityUpdate.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly ApiDtoPropertyAddition apiPropertyAddition;

        public EntityUpdateGeneration(
            EntityCoreAddition entityCoreAddition,
            ApiDtoPropertyAddition apiPropertyAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.apiPropertyAddition = apiPropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackend(entity, ApiProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.apiPropertyAddition.AddPropertyToBackendFile(property, ApiProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.apiPropertyAddition.AddPropertyToBackendFile(relationSide, ApiProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            this.Add1ToNRelationSideTo(new Relation1ToN(relation));
        }
    }
}