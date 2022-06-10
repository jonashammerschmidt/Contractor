﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityUpdatePageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.ts.template.txt");

        private static readonly string FileName = "dialogs\\update\\entity-kebab-update.dialog.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityUpdatePageTsPropertyAddition entityUpdatePageTsPropertyAddition;
        private readonly EntityUpdatePageTsToPropertyAddition entityUpdatePageTsToPropertyAddition;

        public EntityUpdatePageTsGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityUpdatePageTsPropertyAddition entityUpdatePageTsPropertyAddition,
            EntityUpdatePageTsToPropertyAddition entityUpdatePageTsToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityUpdatePageTsPropertyAddition = entityUpdatePageTsPropertyAddition;
            this.entityUpdatePageTsToPropertyAddition = entityUpdatePageTsToPropertyAddition;
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
            this.entityUpdatePageTsPropertyAddition.Edit(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.entityUpdatePageTsToPropertyAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.entityUpdatePageTsToPropertyAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}