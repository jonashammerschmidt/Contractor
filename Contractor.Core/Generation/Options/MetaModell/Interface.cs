using System.Collections.Generic;

namespace Contractor.Core.MetaModell
{
    public class Interface
    {
        public string Name { get; set; }

        public List<InterfaceProperty> Properties { get; set; } = new();

        public List<InterfaceRelation> Relations { get; set; } = new();
    }
}