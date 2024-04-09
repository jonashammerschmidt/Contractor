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
                        if ((relation1To1.TargetEntity.HasScope && !relation1To1.SourceEntity.HasScope) || relation1To1.SourceEntity.HasOtherScope(relation1To1.TargetEntity))
                        {
                            Relation1ToN scopeRelation1ToN = new Relation1ToN()
                            {
                                TargetEntityName = relation1To1.TargetEntity.ScopeEntity.Name,
                                PropertyNameInSource = (relation1To1.PropertyNameInSource ?? relation1To1.TargetEntity.Name) + relation1To1.TargetEntity.ScopeEntity.Name,
                                PropertyNameInTarget = (relation1To1.PropertyNameInTarget ?? relation1To1.SourceEntity.Name) + relation1To1.TargetEntity.Name,
                                IsOptional = relation1To1.IsOptional,
                                OnDelete = "NoAction",
                                Order = relation1To1.Order,
                                IsCreatedByPreProcessor = true,
                                RelationBeforePreProcessor = relation1To1,
                            };
                            scopeRelation1ToN.AddLinks(entity);
                            entity.Relations1ToN = entity.Relations1ToN
                                .Concat(new List<Relation1ToN>() { scopeRelation1ToN })
                                .ToList();
                        }
                    }

                    foreach (var relation1ToN in entity.Relations1ToN)
                    {
                        if ((relation1ToN.TargetEntity.HasScope && !relation1ToN.SourceEntity.HasScope) || relation1ToN.SourceEntity.HasOtherScope(relation1ToN.TargetEntity))
                        {
                            Relation1ToN scopeRelation1ToN = new Relation1ToN()
                            {
                                TargetEntityName = relation1ToN.TargetEntity.ScopeEntity.Name,
                                PropertyNameInSource = (relation1ToN.PropertyNameInSource ?? relation1ToN.TargetEntity.Name) + relation1ToN.TargetEntity.ScopeEntity.Name,
                                PropertyNameInTarget = (relation1ToN.PropertyNameInTarget ?? relation1ToN.SourceEntity.Name) + relation1ToN.TargetEntity.Name,
                                IsOptional = relation1ToN.IsOptional,
                                OnDelete = "NoAction",
                                Order = relation1ToN.Order,
                                IsCreatedByPreProcessor = true,
                                RelationBeforePreProcessor = relation1ToN,
                            };
                            scopeRelation1ToN.AddLinks(entity);
                            entity.Relations1ToN = entity.Relations1ToN
                                .Concat(new List<Relation1ToN>() { scopeRelation1ToN })
                                .ToList();
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
                    .Select(relation => relation.TargetEntity);
                var dependencies1ToN = entity.Relations1ToN
                    .Where(relation => !excludeOptionalRelations || !relation.IsOptional)
                    .Select(relation => relation.TargetEntity);
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