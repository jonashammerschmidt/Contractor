using Contractor.Core;
using System.Collections.Generic;

namespace Contractor.Core
{
    public class Entity
    {
        public string Name { get; set; }

        public string NamePlural { get; set; }

        public string ScopeEntityName { get; set; }

        public bool HasScope
        {
            get { return string.IsNullOrWhiteSpace(this.ScopeEntityName); }
        }

        public IEnumerable<Property> Properties { get; set; }

        public IEnumerable<Relation1ToN> Relations1ToN { get; set; }

        public IEnumerable<Relation1To1> Relations1To1 { get; set; }

        public IEnumerable<Index> Indices { get; set; }

        public Module Module { get; private set; }

        public void AddLinks(Module module)
        {
            this.Module = module;

            foreach (var property in this.Properties)
            {
                property.AddLinks(this);
            }

            foreach (var relation1ToN in this.Relations1ToN)
            {
                relation1ToN.AddLinks(this);
            }

            foreach (var relation1To1 in this.Relations1To1)
            {
                relation1To1.AddLinks(this);
            }

            foreach (var index in this.Indices)
            {
                index.AddLinks(this);
            }
        }

        public Property FindProperty(string propertyName)
        {
            foreach (var property in Properties)
            {
                if (property.Name.ToLower() == propertyName)
                {
                    return property;
                }
            }

            throw new KeyNotFoundException(propertyName);
        }
    }
}