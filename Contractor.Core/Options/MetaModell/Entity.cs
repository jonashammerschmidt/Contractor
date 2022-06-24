﻿using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class Entity
    {
        private string name;
        private string namePlural;
        private string scopeEntityName;

        public string Name
        {
            get { return name; }
            set { name = value.ToVariableName(); }
        }

        public string NameLower
        {
            get { return Name.LowerFirstChar(); }
        }

        public string NameKebab
        {
            get { return Name.PascalToKebabCase(); }
        }

        public string NameReadable
        {
            get { return Name.ToReadable(); }
        }

        public string NamePlural
        {
            get { return namePlural; }
            set { namePlural = value.ToVariableName(); }
        }

        public string NamePluralLower
        {
            get { return NamePlural.LowerFirstChar(); }
        }

        public string NamePluralKebab
        {
            get { return NamePlural.PascalToKebabCase(); }
        }

        public string NamePluralReadable
        {
            get { return NamePlural.ToReadable(); }
        }

        public string ScopeEntityName
        {
            private get { return scopeEntityName; }
            set { scopeEntityName = value?.ToVariableName(); }
        }

        public bool HasScope
        {
            get { return !string.IsNullOrWhiteSpace(ScopeEntityName); }
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
            Module = module;
            if (ScopeEntityName != null)
            {
                ScopeEntity = module.Options.FindEntity(ScopeEntityName);
            }
        }

        public void AddLinksForChildren()
        {
            foreach (var property in Properties)
            {
                property.AddLinks(this);
            }

            foreach (var relation1ToN in Relations1ToN)
            {
                relation1ToN.AddLinks(this);
            }

            foreach (var relation1To1 in Relations1To1)
            {
                relation1To1.AddLinks(this);
            }

            foreach (var index in Indices)
            {
                index.AddLinks(this);
            }

            DisplayProperty =
                Properties.FirstOrDefault(property => property.IsDisplayProperty) ??
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