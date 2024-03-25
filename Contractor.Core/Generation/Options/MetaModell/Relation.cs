using Contractor.Core.Helpers;

namespace Contractor.Core.MetaModell
{
    public abstract class Relation
    {
        private string entityNameFrom;
        protected string propertyNameFrom;
        private string propertyNameTo;

        protected Relation()
        {
        }

        protected Relation(Relation relation)
        {
            entityNameFrom = relation.entityNameFrom;
            propertyNameFrom = relation.propertyNameFrom;
            propertyNameTo = relation.propertyNameTo;

            IsOptional = relation.IsOptional;
            OnDelete = relation.OnDelete;
            IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor;
            RelationBeforePreProcessor = relation.RelationBeforePreProcessor;
            Order = relation.Order;
            EntityFrom = relation.EntityFrom;
            EntityTo = relation.EntityTo;
        }

        public string EntityNameFrom
        {
            set { entityNameFrom = value.ToVariableName(); }
        }

        public string PropertyNameFrom
        {
            get { return propertyNameFrom; }
            set { propertyNameFrom = value?.ToVariableName(); }
        }

        public string PropertyNameTo
        {
            get { return propertyNameTo; }
            set { propertyNameTo = value?.ToVariableName(); }
        }

        public bool IsOptional { get; set; }

        public int Order { get; set; }

        public bool IsCreatedByPreProcessor { get; internal set; }

        public Relation RelationBeforePreProcessor { get; internal set; }

        public string OnDelete { get; set; }

        public Entity EntityFrom { get; set; }

        public Entity EntityTo { get; set; }

        public void AddLinks(Entity entity)
        {
            EntityTo = entity;
            EntityFrom = entity.Module.Options.FindEntity(entityNameFrom);
        }
    }
}