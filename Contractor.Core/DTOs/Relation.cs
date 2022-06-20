using Contractor.Core.Helpers;

namespace Contractor.Core
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
            this.entityNameFrom = relation.entityNameFrom;
            this.propertyNameFrom = relation.propertyNameFrom;
            this.propertyNameTo = relation.propertyNameTo;

            this.IsOptional = relation.IsOptional;
            this.OnDelete = relation.OnDelete;
            this.Order = relation.Order;
            this.EntityFrom = relation.EntityFrom;
            this.EntityTo = relation.EntityTo;
        }

        public string EntityNameFrom
        {
            set { this.entityNameFrom = value.ToVariableName(); }
        }

        public string PropertyNameFrom
        {
            get { return this.propertyNameFrom; }
            set { this.propertyNameFrom = value?.ToVariableName(); }
        }

        public string PropertyNameTo
        {
            get { return propertyNameTo; }
            set { this.propertyNameTo = value?.ToVariableName(); }
        }

        public bool IsOptional { get; set; }

        public int Order { get; set; }

        public string OnDelete { get; set; }

        public Entity EntityFrom { get; private set; }

        public Entity EntityTo { get; private set; }

        public void AddLinks(Entity entity)
        {
            this.EntityTo = entity;
            this.EntityFrom = entity.Module.Options.FindEntity(entityNameFrom);
        }
    }
}