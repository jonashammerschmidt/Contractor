using Contractor.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core
{
    public class ContractorPreprocessor
    {
        public static IEnumerable<Entity> PreProcess(
            ContractorGenerationOptions contractorGenerationOptions)
        {
            foreach (var module in contractorGenerationOptions.Modules.Where(module => !module.Skip))
            {
                foreach (Entity entity in module.Entities.Where(entity => !entity.Skip))
                {
                    foreach (var relation1To1 in entity.Relations1To1)
                    {
                        if (relation1To1.EntityFrom.HasScope && !relation1To1.EntityTo.HasScope)
                        {
                            Relation1ToN scopeRelation1ToN = new Relation1ToN()
                            {
                                EntityNameFrom = relation1To1.EntityFrom.ScopeEntity.Name,
                                PropertyNameFrom = relation1To1.PropertyNameFrom + relation1To1.EntityFrom.ScopeEntity.Name,
                                PropertyNameTo = relation1To1.PropertyNameTo + relation1To1.EntityFrom.Name,
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
                        if (relation1ToN.EntityFrom.HasScope && !relation1ToN.EntityTo.HasScope)
                        {
                            Relation1ToN scopeRelation1ToN = new Relation1ToN()
                            {
                                EntityNameFrom = relation1ToN.EntityFrom.ScopeEntity.Name,
                                PropertyNameFrom = relation1ToN.PropertyNameFrom + relation1ToN.EntityFrom.ScopeEntity.Name,
                                PropertyNameTo = relation1ToN.PropertyNameTo + relation1ToN.EntityFrom.Name,
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

            return SortEntities(contractorGenerationOptions);
        }

        private static IEnumerable<Entity> SortEntities(ContractorGenerationOptions contractorGenerationOptions)
        {
            IEnumerable<Entity> entities = contractorGenerationOptions.Modules
                .SelectMany(module => module.Entities);

            Func<Entity, IEnumerable<Entity>> dependencyBuilder = (entity) =>
                {
                    var dependencies1To1 = entity.Relations1To1.Select(relation => relation.EntityFrom);
                    var dependencies1ToN = entity.Relations1ToN.Select(relation => relation.EntityFrom);
                    return dependencies1To1.Concat(dependencies1ToN);
                };

            IEnumerable<Entity> sortedEntities = SortingByDependency.Sort(entities, dependencyBuilder, true);

            contractorGenerationOptions.Sort(sortedEntities);
            return sortedEntities;
        }
    }
}