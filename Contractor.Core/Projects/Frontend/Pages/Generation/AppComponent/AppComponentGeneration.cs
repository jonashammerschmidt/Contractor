using Contractor.Core.MetaModell;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class AppComponentGeneration : ClassGeneration
    {
        private readonly AppComponentModuleAddition appComponentModuleAddition;
        private readonly AppComponentEntityAddition appComponentEntityAddition;

        public AppComponentGeneration(
            AppComponentModuleAddition appComponentModuleAddition,
            AppComponentEntityAddition appComponentEntityAddition)
        {
            this.appComponentModuleAddition = appComponentModuleAddition;
            this.appComponentEntityAddition = appComponentEntityAddition;
        }

        protected override void AddModuleActions(Module module)
        {
            this.appComponentModuleAddition.Add(module);
        }

        protected override void AddEntity(Entity entity)
        {
            this.appComponentEntityAddition.Add(entity);
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