﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityDetailPageScssGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.scss.template.txt");

        private static readonly string FileName = "sub-pages\\detail\\entity-kebab-detail.page.scss";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;

        public EntityDetailPageScssGeneration(
            FrontendEntityAddition frontendEntityCoreAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntity(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
        }
    }
}