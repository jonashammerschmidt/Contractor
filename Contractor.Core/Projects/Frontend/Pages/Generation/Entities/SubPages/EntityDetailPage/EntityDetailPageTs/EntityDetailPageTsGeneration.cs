﻿using Contractor.Core.MetaModell;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityDetailPageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.ts.template.txt");

        private static readonly string FileName = "sub-pages\\detail\\entity-kebab-detail.page.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntityDetailPageTsFromPropertyAddition entityDetailPageTsFromPropertyAddition;

        public EntityDetailPageTsGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntityDetailPageTsFromPropertyAddition entityDetailPageTsFromPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityDetailPageTsFromPropertyAddition = entityDetailPageTsFromPropertyAddition;
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

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            this.entityDetailPageTsFromPropertyAddition.AddRelationSideToFrontendFile(relationSideFrom, PagesProjectGeneration.DomainFolder, FileName);
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