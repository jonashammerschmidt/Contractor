﻿using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    public class EntitiesCrudLogicGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntitiesCrudLogicTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogic.cs";

        private readonly EntityCoreAddition entityCoreAddition;

        public EntitiesCrudLogicGeneration(
            EntityCoreAddition entityCoreAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToBackend(entity, LogicProjectGeneration.DomainFolder, templatePath, FileName);
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
    }
}