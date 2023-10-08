using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityCreatePageTsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-create.page.ts.template.txt");

        private static readonly string FileName = Path.Combine("dialogs", "create", "entity-kebab-create.dialog.ts");

        private readonly EntityCoreAddition frontendEntityCoreAddition;
        private readonly EntityCreatePageTsPropertyAddition entityCreatePageTsPropertyAddition;
        private readonly EntityCreatePageTsToPropertyAddition entityCreatePageTsToPropertyAddition;

        public EntityCreatePageTsGeneration(
            EntityCoreAddition frontendEntityCoreAddition,
            EntityCreatePageTsPropertyAddition entityCreatePageTsPropertyAddition,
            EntityCreatePageTsToPropertyAddition entityCreatePageTsToPropertyAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entityCreatePageTsPropertyAddition = entityCreatePageTsPropertyAddition;
            this.entityCreatePageTsToPropertyAddition = entityCreatePageTsToPropertyAddition;
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
            this.entityCreatePageTsPropertyAddition.AddPropertyToFrontendFile(property, PagesProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.entityCreatePageTsToPropertyAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
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