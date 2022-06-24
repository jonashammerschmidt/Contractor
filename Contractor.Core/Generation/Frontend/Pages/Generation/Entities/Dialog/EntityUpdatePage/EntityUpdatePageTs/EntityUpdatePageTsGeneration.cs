using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityUpdatePageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.ts.template.txt");

        private static readonly string FileName = "dialogs\\update\\entity-kebab-update.dialog.ts";

        private readonly EntityCoreAddition frontendEntityCoreAddition;
        private readonly EntityUpdatePageTsPropertyAddition entityUpdatePageTsPropertyAddition;
        private readonly EntityUpdatePageTsToPropertyAddition entityUpdatePageTsToPropertyAddition;

        public EntityUpdatePageTsGeneration(
            EntityCoreAddition frontendEntityCoreAddition,
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
            this.frontendEntityCoreAddition.AddEntityToFrontend(entity, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.entityUpdatePageTsPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityUpdatePageTsToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityUpdatePageTsToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
        }
    }
}