﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityPageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab.page.html.template.txt");

        private static readonly string FileName = "entities-kebab.page.html";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntitiesPageHtmlPropertyAddition entitiesPageHtmlPropertyAddition;
        private readonly EntitiesPageHtmlToRelationAddition entitiesPageHtmlToRelationAddition;
        private readonly EntitiesPageHtmlFromOneToOneRelationAddition entitiesPageHtmlFromOneToOneRelationAddition;
        private readonly EntitiesPageHtmlToOneToOneRelationAddition entitiesPageHtmlToOneToOneRelationAddition;

        public EntityPageHtmlGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntitiesPageHtmlPropertyAddition entitiesPageHtmlPropertyAddition,
            EntitiesPageHtmlToRelationAddition entitiesPageHtmlToRelationAddition,
            EntitiesPageHtmlFromOneToOneRelationAddition entitiesPageHtmlFromOneToOneRelationAddition,
            EntitiesPageHtmlToOneToOneRelationAddition entitiesPageHtmlToOneToOneRelationAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entitiesPageHtmlPropertyAddition = entitiesPageHtmlPropertyAddition;
            this.entitiesPageHtmlToRelationAddition = entitiesPageHtmlToRelationAddition;
            this.entitiesPageHtmlFromOneToOneRelationAddition = entitiesPageHtmlFromOneToOneRelationAddition;
            this.entitiesPageHtmlToOneToOneRelationAddition = entitiesPageHtmlToOneToOneRelationAddition;
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
            this.entitiesPageHtmlPropertyAddition.Edit(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.entitiesPageHtmlToRelationAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            // From
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            this.entitiesPageHtmlFromOneToOneRelationAddition.Edit(relationSideFrom, PagesProjectGeneration.DomainFolder, FileName);

            // To
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.entitiesPageHtmlToOneToOneRelationAddition.Edit(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}