using System.Transactions;

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

        public Relation1ToN(Entity targetEntity, Entity sourceEntity)
        {
            this.TargetEntity = targetEntity;
            this.SourceEntity = sourceEntity;
            this.TargetEntityName = targetEntity.Name;
            this.PropertyNameInSource = targetEntity.Name;
            this.PropertyNameInTarget = sourceEntity.NamePlural;
            this.IsOptional = false;
            this.OnDelete = "NoAction";
            this.Order = -1;
            this.IsCreatedByPreProcessor = true;
        }

        public override void AddLinks(Entity entity)
        {
            base.AddLinks(entity);

            this.PropertyNameInSource ??= TargetEntity.Name;
            this.PropertyNameInTarget ??= SourceEntity.NamePlural;
        }
    }
}
