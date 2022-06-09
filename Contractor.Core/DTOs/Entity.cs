using Contractor.Core;
using Contractor.Core.Helpers;
using System.Collections.Generic;

namespace Contractor.Core
{
    public class Entity
    {
        public string Name { get; set; }

        public string NameLower
        {
            get { return Name.LowerFirstChar(); }
        }

        public string NameKebab
        {
            get { return StringConverter.PascalToKebabCase(Name); }
        }

        public string NameReadable
        {
            get { return Name.ToReadable(); }
        }

        public string NamePlural { get; set; }

        public string NamePluralLower
        {
            get { return NamePlural.LowerFirstChar(); }
        }

        public string NamePluralKebab
        {
            get { return StringConverter.PascalToKebabCase(NamePlural); }
        }

        public string NamePluralReadable
        {
            get { return NamePlural.ToReadable(); }
        }

        public string ScopeEntityName { private get; set; }

        public bool HasScope
        {
            get { return string.IsNullOrWhiteSpace(this.ScopeEntityName); }
        }

        public Entity ScopeEntity { get; private set; }

        public IEnumerable<Property> Properties { get; set; }

        public IEnumerable<Relation1ToN> Relations1ToN { get; set; }

        public IEnumerable<Relation1To1> Relations1To1 { get; set; }

        public IEnumerable<Index> Indices { get; set; }

        public Module Module { get; private set; }

        public void AddLinks(Module module)
        {
            this.Module = module;
            this.ScopeEntity = module.Options.FindEntity(this.ScopeEntityName);

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