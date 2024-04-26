using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class Index
    {
        private IEnumerable<string> columnNames;
        private IEnumerable<string> includeNames;

        public string PropertyNames
        {
            set { columnNames = value.Split(',').Select(propertyName => propertyName.ToVariableName()); }
        }

        public string Includes
        {
            set { includeNames = value?.Split(',').Select(propertyName => propertyName.ToVariableName()) ?? new string[] { }; }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return columnNames; }
        }

        public IEnumerable<string> IncludeNames
        {
            get { return includeNames; }
        }

        public bool IsClustered { get; set; }

        public bool IsUnique { get; set; }

        public string Where { get; set; }

        public Entity Entity { get; private set; }

        public void AddLinks(Entity entity)
        {
            Entity = entity;
        }
    }
}