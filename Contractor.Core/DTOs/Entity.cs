﻿using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core
{
    public class Entity
    {
        private string name;
        private string namePlural;
        private string scopeEntityName;

        public string Name
        {
            get { return name; }
            set { this.name = value.ToVariableName(); }
        }

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

        public string NamePlural
        {
            get { return namePlural; }
            set { this.namePlural = value.ToVariableName(); }
        }

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

        public string ScopeEntityName
        {
            private get { return scopeEntityName; }
            set { this.scopeEntityName = value?.ToVariableName(); }
        }

        public bool HasScope
        {
            get { return !string.IsNullOrWhiteSpace(this.ScopeEntityName); }
        }

        public bool Skip { get; set; }

        public string IdType { get; set; }

        public Entity ScopeEntity { get; private set; }

        public Property DisplayProperty { get; set; }

        public IEnumerable<Property> Properties { get; set; }

        public IEnumerable<Relation1ToN> Relations1ToN { get; set; }

        public IEnumerable<Relation1To1> Relations1To1 { get; set; }

        public IEnumerable<Index> Indices { get; set; }

        public Module Module { get; private set; }

        public void AddLinks(Module module)
        {
            this.Module = module;
            if (this.ScopeEntityName != null)
            {
                this.ScopeEntity = module.Options.FindEntity(this.ScopeEntityName);
            }
        }

        public void AddLinksForChildren(Module module)
        {
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

            this.DisplayProperty =
                this.Properties.FirstOrDefault(property => property.IsDisplayProperty) ??
                FindProperty("Bezeichnung", true) ??
                FindProperty("Name", true) ??
                new Property()
                {
                    Name = "Id",
                    IsDisplayProperty = true,
                    Type = "Guid",
                };
        }

        public Property FindProperty(string propertyName)
        {
            return FindProperty(propertyName, false);
        }

        public Property FindProperty(string propertyName, bool nullable)
        {
            foreach (var property in Properties)
            {
                if (property.Name.ToLower() == propertyName.ToLower())
                {
                    return property;
                }
            }

            if (!nullable)
            {
                throw new KeyNotFoundException(propertyName);
            }

            return null;
        }
    }
}