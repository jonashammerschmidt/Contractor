using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class Index
    {
        private IEnumerable<string> columnNames;

        public string PropertyNames
        {
            set { columnNames = value.Split(',').Select(propertyName => propertyName.ToVariableName()); }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return columnNames; }
        }

        public bool IsClustered { get; set; }

        public bool IsUnique { get; set; }

        public Entity Entity { get; private set; }

        public void AddLinks(Entity entity)
        {
            Entity = entity;
        }
    }
}