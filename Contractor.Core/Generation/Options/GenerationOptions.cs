using Contractor.Core.Generation;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class GenerationOptions
    {
        public Paths Paths { get; set; }

        public List<Replacement> Replacements { get; set; }

        public List<Module> Modules { get; set; }

        public List<PurposeDto> PurposeDtos { get; set; }

        public List<Interface> Interfaces { get; set; }

        public bool IsVerbose { get; set; }

        public List<ClassGenerationTag> Tags { get; set; }

        public void AddLinks()
        {
            foreach (var module in Modules)
            {
                module.AddLinks(this);
            }

            foreach (var module in Modules)
            {
                module.AddLinksForChildren();
            }

            foreach (var purposeDto in PurposeDtos)
            {
                purposeDto.AddLinks(this);
            }
        }

        public Entity FindEntity(string entityName)
        {
            foreach (var module in Modules)
            {
                foreach (var entity in module.Entities)
                {
                    if (entity.Name.ToLower() == entityName.ToLower())
                    {
                        return entity;
                    }
                }
            }

            throw new KeyNotFoundException(entityName);
        }
        
        public Relation FindRelation(Entity searchedEntity, string searchedPropertyName)
        {
            Relation relation = null;
            foreach (var module in Modules)
            {
                foreach (var entity in module.Entities)
                {
                    relation = 
                        FindSimpleRelation(searchedEntity, searchedPropertyName, entity) ??
                        FindScopeRelation(searchedEntity, searchedPropertyName, entity);
                    
                    if (relation != null)
                    {
                        return relation;
                    }
                }
            }
            
            relation = FindScopeUnscopedRelation(searchedEntity, searchedPropertyName);
            if (relation != null)
            {
                return relation;
            }

            throw new KeyNotFoundException(searchedEntity.Name + "." + searchedPropertyName);
        }

        private Relation FindSimpleRelation(Entity searchedEntity, string searchedPropertyName, Entity entity)
        {
            var relations1To1From = entity.Relations1To1.SingleOrDefault(relation => 
                relation.EntityFrom.Name == searchedEntity.Name
                && (relation.PropertyNameTo ?? relation.EntityTo.Name).ToLower() == searchedPropertyName.ToLower());
            var relations1To1To = entity.Relations1To1.SingleOrDefault(relation => 
                relation.EntityTo.Name == searchedEntity.Name
                && (relation.PropertyNameFrom ?? relation.EntityFrom.Name).ToLower() == searchedPropertyName.ToLower());
            var relations1ToNFrom = entity.Relations1ToN.SingleOrDefault(relation => 
                relation.EntityFrom.Name == searchedEntity.Name
                && (relation.PropertyNameTo ?? relation.EntityTo.NamePlural).ToLower() == searchedPropertyName.ToLower());
            var relations1ToNTo = entity.Relations1ToN.SingleOrDefault(relation => 
                relation.EntityTo.Name == searchedEntity.Name
                && (relation.PropertyNameFrom ?? relation.EntityFrom.Name).ToLower() == searchedPropertyName.ToLower());
            var foundRelation = (relations1To1From as Relation)
                                ?? (relations1To1To as Relation)
                                ?? (relations1ToNFrom as Relation)
                                ?? (relations1ToNTo as Relation);
            return foundRelation;
        }

        private Relation FindScopeRelation(Entity searchedEntity, string searchedPropertyName, Entity otherEntity)
        {
            if (otherEntity.HasScope && otherEntity == searchedEntity)
            {
                if (otherEntity.ScopeEntity.Name.ToLower() == searchedPropertyName.ToLower())
                {
                    {
                        return new Relation1ToN(otherEntity.ScopeEntity, otherEntity);
                    }
                }
            }

            foreach (var scopedEntity in otherEntity.ScopedEntities)
            {
                if (scopedEntity.NamePlural.ToLower() == searchedPropertyName.ToLower() && otherEntity == searchedEntity)
                {
                    {
                        return new Relation1ToN(otherEntity, scopedEntity);
                    }
                }
            }

            return null;
        }

        private Relation FindScopeUnscopedRelation(Entity searchedEntity, string searchedPropertyName)
        {
            foreach (var relation1To1 in searchedEntity.Relations1To1)
            {
                if ((relation1To1.EntityFrom.HasScope && !relation1To1.EntityTo.HasScope) || relation1To1.EntityTo.HasOtherScope(relation1To1.EntityFrom))
                {
                    Relation1To1 scopeRelation1To1 = new Relation1To1()
                    {
                        EntityFrom = relation1To1.EntityFrom.ScopeEntity,
                        EntityNameFrom = relation1To1.EntityFrom.ScopeEntity.Name,
                        PropertyNameFrom = (relation1To1.PropertyNameFrom ?? relation1To1.EntityFrom.Name) + relation1To1.EntityFrom.ScopeEntity.Name,
                        EntityTo = relation1To1.EntityTo,
                        PropertyNameTo = (relation1To1.PropertyNameTo ?? relation1To1.EntityTo.Name) + relation1To1.EntityFrom.Name,
                        IsOptional = relation1To1.IsOptional,
                        OnDelete = "NoAction",
                        Order = relation1To1.Order,
                        IsCreatedByPreProcessor = true,
                        RelationBeforePreProcessor = relation1To1,
                    };
                    if (scopeRelation1To1.PropertyNameTo == searchedPropertyName)
                    {
                        return scopeRelation1To1;
                    }
                }
            }

            foreach (var relation1ToN in searchedEntity.Relations1ToN)
            {
                if ((relation1ToN.EntityFrom.HasScope && !relation1ToN.EntityTo.HasScope) || relation1ToN.EntityTo.HasOtherScope(relation1ToN.EntityFrom))
                {
                    Relation1ToN scopeRelation1ToN = new Relation1ToN()
                    {
                        EntityFrom = relation1ToN.EntityFrom.ScopeEntity,
                        EntityNameFrom = relation1ToN.EntityFrom.ScopeEntity.Name,
                        PropertyNameFrom = (relation1ToN.PropertyNameFrom ?? relation1ToN.EntityFrom.Name) + relation1ToN.EntityFrom.ScopeEntity.Name,
                        EntityTo = relation1ToN.EntityTo,
                        PropertyNameTo = (relation1ToN.PropertyNameTo ?? relation1ToN.EntityTo.Name) + relation1ToN.EntityFrom.Name,
                        IsOptional = relation1ToN.IsOptional,
                        OnDelete = "NoAction",
                        Order = relation1ToN.Order,
                        IsCreatedByPreProcessor = true,
                        RelationBeforePreProcessor = relation1ToN,
                    };

                    if (scopeRelation1ToN.PropertyNameFrom == searchedPropertyName)
                    {
                        return scopeRelation1ToN;
                    }
                }
            }

            return null;
        }

        public void Sort(IEnumerable<Entity> sortedEntities)
        {
            Modules = sortedEntities
                .Select(entity => entity.Module)
                .Distinct()
                .ToList();

            foreach (var module in Modules)
            {
                module.Sort(sortedEntities);
            }
        }
    }
}