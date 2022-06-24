using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntitiesPagesModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-kebab-pages.module.template.txt");

        private static readonly string FileName = "entities-kebab-pages.module.ts";

        private readonly FrontendEntityAddition frontendEntityCoreAddition;
        private readonly EntitiesPagesModuleToRelationAddition entitiesPagesModuleToRelationAddition;

        public EntitiesPagesModuleGeneration(
            FrontendEntityAddition frontendEntityCoreAddition,
            EntitiesPagesModuleToRelationAddition entitiesPagesModuleToRelationAddition)
        {
            this.frontendEntityCoreAddition = frontendEntityCoreAddition;
            this.entitiesPagesModuleToRelationAddition = entitiesPagesModuleToRelationAddition;
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
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.entitiesPagesModuleToRelationAddition.AddRelationSideToFrontendFile(relationSideTo, PagesProjectGeneration.DomainFolder, FileName);
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