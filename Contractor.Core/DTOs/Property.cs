using System.Collections.Generic;

namespace Contractor.Core
{
    public class Property
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsOptional { get; set; }

        public int Order { get; set; }

        public Entity Entity { get; private set; }

        public void AddLinks(Entity entity)
        {
            this.Entity = entity;
        }
    }
}