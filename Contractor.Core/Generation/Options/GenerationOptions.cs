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

        public List<CustomDto> CustomDtos { get; set; }

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

            foreach (var customDto in CustomDtos)
            {
                customDto.AddLinks(this);
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
            foreach (var module in Modules)
            {
                foreach (var entity in module.Entities)
                {
                    var relations1To1From = entity.Relations1To1.SingleOrDefault(relation => relation.EntityFrom.Name == searchedEntity.Name && relation.PropertyNameTo.ToLower() == searchedPropertyName.ToLower());
                    var relations1To1To = entity.Relations1To1.SingleOrDefault(relation => relation.EntityTo.Name == searchedEntity.Name && relation.PropertyNameFrom.ToLower() == searchedPropertyName.ToLower());
                    var relations1ToNFrom = entity.Relations1ToN.SingleOrDefault(relation => relation.EntityFrom.Name == searchedEntity.Name && relation.PropertyNameTo.ToLower() == searchedPropertyName.ToLower());
                    var relations1ToNTo = entity.Relations1ToN.SingleOrDefault(relation => relation.EntityTo.Name == searchedEntity.Name && relation.PropertyNameFrom.ToLower() == searchedPropertyName.ToLower());
                    var foundRelation = (relations1To1From as Relation) ?? (relations1To1To as Relation) ?? (relations1ToNFrom as Relation) ?? (relations1ToNTo as Relation);
                    if (foundRelation != null)
                    {
                        return foundRelation;
                    }

                    if (entity.HasScope)
                    {
                        if (entity.ScopeEntity.NameLower == searchedPropertyName.ToLower())
                        {
                            return new Relation1ToN(entity.ScopeEntity, entity);
                        }
                    }

                    foreach (var scopedEntity in entity.ScopedEntities)
                    {
                        if (scopedEntity.NamePluralLower == searchedPropertyName.ToLower())
                        {
                            return new Relation1ToN(entity.ScopeEntity, scopedEntity);
                        }
                    }
                }
            }


            throw new KeyNotFoundException(searchedEntity.Name + "." + searchedPropertyName);
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