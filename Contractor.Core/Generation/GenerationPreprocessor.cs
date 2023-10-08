using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core
{
    public class GenerationPreprocessor
    {
        public static IEnumerable<Entity> PreProcess(
            GenerationOptions generationOptions)
        {
            foreach (var module in generationOptions.Modules.Where(module => !module.Skip))
            {
                foreach (Entity entity in module.Entities.Where(entity => !entity.Skip))
                {
                    foreach (var relation1To1 in entity.Relations1To1)
                    {
                        if ((relation1To1.EntityFrom.HasScope && !relation1To1.EntityTo.HasScope) || relation1To1.EntityTo.HasOtherScope(relation1To1.EntityFrom))
                        {
                            Relation1ToN scopeRelation1ToN = new Relation1ToN()
                            {
                                EntityNameFrom = relation1To1.EntityFrom.ScopeEntity.Name,
                                PropertyNameFrom = (relation1To1.PropertyNameFrom ?? relation1To1.EntityFrom.Name) + relation1To1.EntityFrom.ScopeEntity.Name,
                                PropertyNameTo = (relation1To1.PropertyNameTo ?? relation1To1.EntityTo.Name) + relation1To1.EntityFrom.Name,
                                IsOptional = relation1To1.IsOptional,
                                OnDelete = "NoAction",
                                Order = relation1To1.Order,
                                IsCreatedByPreProcessor = true,
                                RelationBeforePreProcessor = relation1To1,
                            };
                            scopeRelation1ToN.AddLinks(entity);
                            entity.Relations1ToN = entity.Relations1ToN.Concat(new List<Relation1ToN>() { scopeRelation1ToN });
                        }
                    }

                    foreach (var relation1ToN in entity.Relations1ToN)
                    {
                        if ((relation1ToN.EntityFrom.HasScope && !relation1ToN.EntityTo.HasScope) || relation1ToN.EntityTo.HasOtherScope(relation1ToN.EntityFrom))
                        {
                            Relation1ToN scopeRelation1ToN = new Relation1ToN()
                            {
                                EntityNameFrom = relation1ToN.EntityFrom.ScopeEntity.Name,
                                PropertyNameFrom = (relation1ToN.PropertyNameFrom ?? relation1ToN.EntityFrom.Name) + relation1ToN.EntityFrom.ScopeEntity.Name,
                                PropertyNameTo = (relation1ToN.PropertyNameTo ?? relation1ToN.EntityTo.Name) + relation1ToN.EntityFrom.Name,
                                IsOptional = relation1ToN.IsOptional,
                                OnDelete = "NoAction",
                                Order = relation1ToN.Order,
                                IsCreatedByPreProcessor = true,
                                RelationBeforePreProcessor = relation1ToN,
                            };
                            scopeRelation1ToN.AddLinks(entity);
                            entity.Relations1ToN = entity.Relations1ToN.Concat(new List<Relation1ToN>() { scopeRelation1ToN });
                        }
                    }
                }
            }

            return SortEntities(generationOptions);
        }

        private static IEnumerable<Entity> SortEntities(GenerationOptions generationOptions)
        {
            IEnumerable<Entity> entities = generationOptions.Modules
                .SelectMany(module => module.Entities);

            var circularRelationPath = DependencyCycleHelper.FindCycle(entities, GetDependencyBuilder(true));
            if (circularRelationPath != null)
            {
                string cycleName = string.Join(" -> ", circularRelationPath.Select(cycleItem => cycleItem.ToString()));
                throw new ApplicationException("Circular relation path found, where every relation is mandtory: " + cycleName);
            }

            IEnumerable<Entity> sortedEntities = DependencySortHelper.Sort(entities, GetDependencyBuilder(false));

            generationOptions.Sort(sortedEntities);
            return sortedEntities;
        }

        private static Func<Entity, IEnumerable<Entity>> GetDependencyBuilder(bool excludeOptionalRelations)
        {
            return (entity) =>
            {
                var dependencies1To1 = entity.Relations1To1
                    .Where(relation => !excludeOptionalRelations || !relation.IsOptional)
                    .Select(relation => relation.EntityFrom);
                var dependencies1ToN = entity.Relations1ToN
                    .Where(relation => !excludeOptionalRelations || !relation.IsOptional)
                    .Select(relation => relation.EntityFrom);
                var dependencies = dependencies1To1
                    .Concat(dependencies1ToN)
                    .Where(dependency => dependency != entity);

                if (entity.HasScope)
                {
                    dependencies = dependencies.Append(entity.ScopeEntity);
                }

                return dependencies;
            };
        }
    }
}