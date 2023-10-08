using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class Check
    {
        public string Name { get; set; }

        public string Query { get; set; }

        public Entity Entity { get; private set; }

        public void AddLinks(Entity entity)
        {
            Entity = entity;
        }
    }
}