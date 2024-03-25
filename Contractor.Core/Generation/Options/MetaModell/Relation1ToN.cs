namespace Contractor.Core.MetaModell
{
    public class Relation1ToN : Relation
    {
        public Relation1ToN() : base()
        {
        }

        public Relation1ToN(Relation relation) : base(relation)
        {
        }

        public Relation1ToN(Entity scopeEntity, Entity scopedEntity)
        {
            this.EntityNameFrom = scopeEntity.Name;
            this.PropertyNameFrom = scopeEntity.Name;
            this.PropertyNameTo = scopedEntity.NamePlural;
            this.IsOptional = false;
            this.OnDelete = "NoAction";
            this.Order = -1;
            this.IsCreatedByPreProcessor = true;
        }
    }
}