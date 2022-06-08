﻿using Contractor.Core;
using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core
{
    public class Index
    {
        private IEnumerable<string> propertyNames;

        public string PropertyNames
        {
            set { propertyNames = value.Split(',').Select(propertyName => propertyName.Trim().UpperFirstChar()); }
        }

        public IEnumerable<Property> Properties { get; private set; }

        public bool IsClustered { get; set; }

        public bool IsUnique { get; set; }

        public Entity Entity { get; private set; }

        public void AddLinks(Entity entity)
        {
            Entity = entity;

            Properties = propertyNames.Select(propertyName => entity.FindProperty(propertyName));
        }
    }
}