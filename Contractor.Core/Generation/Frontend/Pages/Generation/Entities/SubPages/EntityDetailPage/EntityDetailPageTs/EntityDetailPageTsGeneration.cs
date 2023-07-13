using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityDetailPageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-detail.page.ts.template.txt");

        private static readonly string FileName = Path.Combine("sub-pages", "detail", "entity-kebab-detail.page.ts");

        private readonly EntityCoreAddition frontendEntityCoreAddition;
        private readonly EntityDetailPageTsPropertyAddition entityDetailPageTsPropertyAddition;
        private readonly EntityDetailPageTsToPropertyAddition entityDetailPageTsToPropertyAddition;

        public EntityDetailPageTsGeneration(
            EntityCoreAddition frontendEntityCoreAddition,
            EntityDetailPageTsPropertyAddition entityDetailPageTsPropertyAddition,
            EntityDetailPageTsToPropertyAddition entityDetailPageTsToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityDetailPageTsPropertyAddition = entityDetailPageTsPropertyAddition;
            this.entityDetailPageTsToPropertyAddition  = entityDetailPageTsToPropertyAddition;
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
            this.entityDetailPageTsPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityDetailPageTsToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
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