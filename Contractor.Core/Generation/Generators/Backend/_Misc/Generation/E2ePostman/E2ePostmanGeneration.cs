using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Backend.Misc
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_MISC })]
    public class E2ePostmanGeneration : ClassGeneration
    {
        private static readonly string FileName = "e2e-postman.json";

        private readonly E2ePostmanEntityAddition e2ePostmanEntityAddition;
        private readonly E2ePostmanPropertyAddition e2ePostmanPropertyAddition;
        private readonly E2ePostmanRelationSideAddition e2ePostmanRelationSideAddition;

        public E2ePostmanGeneration(
            E2ePostmanEntityAddition e2ePostmanEntityAddition,
            E2ePostmanPropertyAddition e2ePostmanPropertyAddition,
            E2ePostmanRelationSideAddition e2ePostmanRelationSideAddition)
        {
            this.e2ePostmanEntityAddition = e2ePostmanEntityAddition;
            this.e2ePostmanPropertyAddition = e2ePostmanPropertyAddition;
            this.e2ePostmanRelationSideAddition = e2ePostmanRelationSideAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.e2ePostmanEntityAddition.AddEntityToBackendFile(entity, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.e2ePostmanPropertyAddition.AddPropertyToBackendFile(property, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            var relationSide = RelationSide.FromObjectRelationEndTo(relation, "", "");
            this.e2ePostmanRelationSideAddition.AddRelationSideToBackendFile(relationSide, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            var relationSide = RelationSide.FromObjectRelationEndTo(relation, "", "");
            this.e2ePostmanRelationSideAddition.AddRelationSideToBackendFile(relationSide, FileName);
        }
    }
}