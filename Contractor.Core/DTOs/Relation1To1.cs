using Contractor.Core.Helpers;

namespace Contractor.Core
{
    public class Relation1To1
    {
        private string entityNameFrom;
        private string propertyNameFrom;
        private string propertyNameTo;

        public string EntityNameFrom
        {
            set { this.entityNameFrom = value.Trim().UpperFirstChar(); }
        }

        public string PropertyNameFrom {
            get { return propertyNameFrom ?? EntityFrom.Name; }
            set { this.propertyNameFrom = value.Trim().UpperFirstChar(); }
        }

        public string PropertyNameTo
        {
            get { return propertyNameTo ?? EntityTo.Name; }
            set { this.propertyNameTo = value.Trim().UpperFirstChar(); }
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