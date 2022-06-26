﻿using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE })]
    internal class DbEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityTemplate.txt");

        private static readonly string FileName = "DbEntity.cs";

        private readonly DbEntityMethodsAddition dbDtoMethodsAddition;
        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public DbEntityGeneration(
            DbEntityMethodsAddition dbDtoMethodsAddition,
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dbDtoMethodsAddition = dbDtoMethodsAddition;
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToBackend(entity, PersistenceProjectGeneration.DtoFolder, templatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToBackendFile(property, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoMethodsAddition.AddPropertyToBackendFile(property, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.propertyAddition.AddPropertyToBackendFile(relationSide, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoMethodsAddition.AddPropertyToBackendFile(relationSide, PersistenceProjectGeneration.DtoFolder, FileName);
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