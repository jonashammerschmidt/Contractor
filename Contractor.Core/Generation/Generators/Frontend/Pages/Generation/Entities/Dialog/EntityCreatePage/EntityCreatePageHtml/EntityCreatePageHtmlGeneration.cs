﻿using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    public class EntityCreatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-create.page.html.template.txt");

        private static readonly string FileName = Path.Combine("dialogs", "create", "entity-kebab-create.dialog.html");

        private readonly EntityCoreAddition frontendEntityCoreAddition;
        private readonly EntityCreatePageHtmlPropertyAddition entityCreatePageHtmlPropertyAddition;
        private readonly EntityCreatePageHtmlToPropertyAddition entityCreatePageHtmlToPropertyAddition;

        public EntityCreatePageHtmlGeneration(
            EntityCoreAddition frontendEntityCoreAddition,
            EntityCreatePageHtmlPropertyAddition entityCreatePageHtmlPropertyAddition,
            EntityCreatePageHtmlToPropertyAddition entityCreatePageHtmlToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityCreatePageHtmlPropertyAddition = entityCreatePageHtmlPropertyAddition;
            this.entityCreatePageHtmlToPropertyAddition = entityCreatePageHtmlToPropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityCoreAddition.AddEntityToFrontend(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.entityCreatePageHtmlPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityCreatePageHtmlToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
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