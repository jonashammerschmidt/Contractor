using Contractor.Core.Helpers;
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

        public List<Entity> ScopedEntities { get; private set; } = new List<Entity>();

        public Property DisplayProperty { get; set; }

        public string DisplayPropertyFallback { get; set; }

        public List<Property> Properties { get; set; }

        public List<Relation1ToN> Relations1ToN { get; set; }

        public List<Relation1To1> Relations1To1 { get; set; }

        public List<Index> Indices { get; set; }

        public List<Check> Checks { get; set; }

        public Module Module { get; private set; }

        public bool HasOtherScope(Entity otherEntity)
        {
            return HasScope && otherEntity.HasScope && ScopeEntity != otherEntity.ScopeEntity;
        }

        public void AddLinks(Module module)
        {
            Module = module;
            if (ScopeEntityName != null)
            {
                ScopeEntity = module.Options.FindEntity(ScopeEntityName);
                ScopeEntity.ScopedEntities.Add(this);
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

            foreach (var check in Checks)
            {
                check.AddLinks(this);
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

            DisplayPropertyFallback =
                Properties.FirstOrDefault(property => property.IsDisplayProperty)?.Name ??
                FindProperty("Bezeichnung", true)?.Name ??
                FindProperty("Name", true)?.Name ??
                Properties.FirstOrDefault()?.Name ??
                (Relations1To1.FirstOrDefault() != null
                    ? (Relations1To1.FirstOrDefault().PropertyNameInSource ?? Relations1To1.FirstOrDefault().TargetEntity.Name) + "Id"
                    : null) ??
                (Relations1ToN.FirstOrDefault() != null
                    ? (Relations1ToN.FirstOrDefault().PropertyNameInSource ?? Relations1ToN.FirstOrDefault().TargetEntity.Name) + "Id"
                    : null) ??
                "Id";
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

        public Property FindPropertyIncludingIds(string propertyName)
        {
            foreach (var property in Properties)
            {
                if (property.Name.ToLower() == propertyName.ToLower())
                {
                    return property;
                }
            }

            var relations1To1IdProperty = this.Relations1To1
                .SingleOrDefault(relation => propertyName == (relation.PropertyNameInSource ?? relation.TargetEntity.Name) + "Id");
            var relations1ToNIdProperty = this.Relations1ToN
                .SingleOrDefault(relation => propertyName == (relation.PropertyNameInSource ?? relation.TargetEntity.Name) + "Id");

            Property idProperty = null;
            if (propertyName == "Id")
            {
                idProperty = new Property()
                {
                    Name = propertyName,
                    Entity = this,
                    Type = "Guid",
                    IsDisplayProperty = this.DisplayProperty.Name == "Id",
                };
            }
            else if (propertyName == this.ScopeEntity?.Name + "Id")
            {
                idProperty = new Property()
                {
                    Name = this.ScopeEntity?.Name + "Id",
                    Type = "Guid",
                };
            }
            else if (relations1To1IdProperty != null)
            {
                idProperty = new Property()
                {
                    Name = (relations1To1IdProperty.PropertyNameInSource ?? relations1To1IdProperty.TargetEntity.Name) + "Id",
                    Type = "Guid",
                    IsOptional = relations1To1IdProperty.IsOptional,
                };
            }
            else if (relations1ToNIdProperty != null)
            {
                idProperty = new Property()
                {
                    Name = (relations1ToNIdProperty.PropertyNameInSource ?? relations1ToNIdProperty.TargetEntity.Name) + "Id",
                    Type = "Guid",
                    IsOptional = relations1ToNIdProperty.IsOptional,
                };
            }
            
            idProperty?.AddLinks(this);

            return idProperty;
        }

        public bool HasPropertiesOrRelations()
        {
            return this.Properties.Count() + this.Relations1To1.Count() + this.Relations1ToN.Count() > 0;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}