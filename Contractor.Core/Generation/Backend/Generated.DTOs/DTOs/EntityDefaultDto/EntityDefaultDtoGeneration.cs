﻿using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_DTOS })]
    internal class EntityDefaultDtoGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDefaultDtoTemplate.txt");

        private static readonly string FileName = "EntityDefaultDto.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly EntityDefaultDtoMethodsAddition entityDefaultDtoMethodsAddition;

        public EntityDefaultDtoGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition propertyAddition,
            EntityDefaultDtoMethodsAddition entityDefaultDtoMethodsAddition)
        {
            this.entityDefaultDtoMethodsAddition = entityDefaultDtoMethodsAddition;
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedDTOsProjectGeneration.DtoFolder, templatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
            this.entityDefaultDtoMethodsAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.propertyAddition.AddPropertyToBackendGeneratedFile(relationSide, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
            this.entityDefaultDtoMethodsAddition.AddPropertyToBackendGeneratedFile(relationSide, GeneratedDTOsProjectGeneration.DtoFolder, FileName);
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