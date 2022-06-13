using Contractor.Core.Helpers;

namespace Contractor.Core
{
    public abstract class Relation
    {

        private string entityNameFrom;
        private string propertyNameFrom;
        private string propertyNameTo;

        protected Relation()
        {
        }

        protected Relation(Relation relation)
        {
            this.entityNameFrom = relation.entityNameFrom;
            this.propertyNameFrom = relation.propertyNameFrom;
            this.propertyNameTo  = relation.propertyNameTo;
            
            this.IsOptional = relation.IsOptional;
            this.Order = relation.Order;
            this.EntityFrom = relation.EntityFrom;
            this.EntityTo = relation.EntityTo;
        }

        public string EntityNameFrom
        {
            set { this.entityNameFrom = value.ToVariableName(); }
        }

        public string PropertyNameFrom {
            get { return propertyNameFrom ?? EntityFrom.Name; }
            set { this.propertyNameFrom = value?.ToVariableName(); }
        }

        public string PropertyNameTo
        {
            get { return propertyNameTo ?? EntityTo.Name; }
            set { this.propertyNameTo = value?.ToVariableName(); }
        }

        public bool IsOptional { get; set; }

        public int Order { get; set; }

        public Entity EntityFrom { get; private set; }

        public Entity EntityTo { get; private set; }

        public void AddLinks(Entity entity)
        {
            this.EntityTo = entity;
            this.EntityFrom = entity.Module.Options.FindEntity(entityNameFrom);
        }
    }
}