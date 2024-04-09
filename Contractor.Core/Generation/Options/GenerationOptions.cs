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
        
        public Relation FindRelation(Entity searchedEntity, string propertyNameInSearchedEntity)
        {
            var relation = FindRelationOrDefault(searchedEntity, propertyNameInSearchedEntity);
            if (relation is null)
            {
                throw new KeyNotFoundException(searchedEntity.Name + "." + propertyNameInSearchedEntity);
            }

            return relation;
        }

        public Relation FindRelationOrDefault(Entity searchedEntity, string propertyNameInSearchedEntity)
        {
            Relation relation = null;
            foreach (var module in Modules)
            {
                foreach (var entity in module.Entities)
                {
                    relation = 
                        FindSimpleRelation(searchedEntity, propertyNameInSearchedEntity, entity) ??
                        FindScopeRelation(searchedEntity, propertyNameInSearchedEntity, entity);
                    
                    if (relation != null)
                    {
                        return relation;
                    }
                }
            }
            
            return FindScopeUnscopedRelation(searchedEntity, propertyNameInSearchedEntity);
        }

        private Relation FindSimpleRelation(Entity entity, string propertyName, Entity otherEntity)
        {
            var relations1To1TargetSide = otherEntity.Relations1To1.SingleOrDefault(relation => 
                relation.TargetEntity.Name == entity.Name
                && (relation.PropertyNameInTarget ?? relation.SourceEntity.Name).ToLower() == propertyName.ToLower());
            var relations1To1SourceSide = otherEntity.Relations1To1.SingleOrDefault(relation => 
                relation.SourceEntity.Name == entity.Name
                && (relation.PropertyNameInSource ?? relation.TargetEntity.Name).ToLower() == propertyName.ToLower());
            var relations1ToNTargetSide = otherEntity.Relations1ToN.SingleOrDefault(relation => 
                relation.TargetEntity.Name == entity.Name
                && (relation.PropertyNameInTarget ?? relation.SourceEntity.NamePlural).ToLower() == propertyName.ToLower());
            var relations1ToNSourceSide = otherEntity.Relations1ToN.SingleOrDefault(relation => 
                relation.SourceEntity.Name == entity.Name
                && (relation.PropertyNameInSource ?? relation.TargetEntity.Name).ToLower() == propertyName.ToLower());
            var foundRelation = (relations1To1TargetSide as Relation)
                                ?? (relations1To1SourceSide as Relation)
                                ?? (relations1ToNTargetSide as Relation)
                                ?? (relations1ToNSourceSide as Relation);
            return foundRelation;
        }

        private Relation FindScopeRelation(Entity searchedEntity, string searchedPropertyName, Entity otherEntity)
        {
            if (otherEntity.HasScope && otherEntity == searchedEntity)
            {
                if (otherEntity.ScopeEntity.Name.ToLower() == searchedPropertyName.ToLower())
                {
                    return new Relation1ToN(otherEntity.ScopeEntity, otherEntity);
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
                if ((relation1To1.TargetEntity.HasScope && !relation1To1.SourceEntity.HasScope) || relation1To1.SourceEntity.HasOtherScope(relation1To1.TargetEntity))
                {
                    Relation1To1 scopeRelation1To1 = new Relation1To1()
                    {
                        TargetEntity = relation1To1.TargetEntity.ScopeEntity,
                        TargetEntityName = relation1To1.TargetEntity.ScopeEntity.Name,
                        PropertyNameInSource = (relation1To1.PropertyNameInSource ?? relation1To1.TargetEntity.Name) + relation1To1.TargetEntity.ScopeEntity.Name,
                        SourceEntity = relation1To1.SourceEntity,
                        PropertyNameInTarget = (relation1To1.PropertyNameInTarget ?? relation1To1.SourceEntity.Name) + relation1To1.TargetEntity.Name,
                        IsOptional = relation1To1.IsOptional,
                        OnDelete = "NoAction",
                        Order = relation1To1.Order,
                        IsCreatedByPreProcessor = true,
                        RelationBeforePreProcessor = relation1To1,
                    };
                    if (scopeRelation1To1.PropertyNameInTarget == searchedPropertyName)
                    {
                        return scopeRelation1To1;
                    }
                }
            }

            foreach (var relation1ToN in searchedEntity.Relations1ToN)
            {
                if ((relation1ToN.TargetEntity.HasScope && !relation1ToN.SourceEntity.HasScope) || relation1ToN.SourceEntity.HasOtherScope(relation1ToN.TargetEntity))
                {
                    Relation1ToN scopeRelation1ToN = new Relation1ToN()
                    {
                        TargetEntity = relation1ToN.TargetEntity.ScopeEntity,
                        TargetEntityName = relation1ToN.TargetEntity.ScopeEntity.Name,
                        PropertyNameInSource = (relation1ToN.PropertyNameInSource ?? relation1ToN.TargetEntity.Name) + relation1ToN.TargetEntity.ScopeEntity.Name,
                        SourceEntity = relation1ToN.SourceEntity,
                        PropertyNameInTarget = (relation1ToN.PropertyNameInTarget ?? relation1ToN.SourceEntity.Name) + relation1ToN.TargetEntity.Name,
                        IsOptional = relation1ToN.IsOptional,
                        OnDelete = "NoAction",
                        Order = relation1ToN.Order,
                        IsCreatedByPreProcessor = true,
                        RelationBeforePreProcessor = relation1ToN,
                    };

                    if (scopeRelation1ToN.PropertyNameInSource == searchedPropertyName)
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