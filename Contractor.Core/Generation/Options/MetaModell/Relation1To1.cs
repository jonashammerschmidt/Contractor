namespace Contractor.Core.MetaModell
{
    public class Relation1To1 : Relation
    {
        public Relation1To1() : base()
        {
        }

        public Relation1To1(Relation relation) : base(relation)
        {
        }

        public override void AddLinks(Entity entity)
        {
            base.AddLinks(entity);

            this.PropertyNameInSource ??= TargetEntity.Name;
            this.PropertyNameInTarget ??= SourceEntity.Name;
        }
    }
}
