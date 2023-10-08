﻿using Contractor.Core.Generation;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class GenerationOptions
    {
        public Paths Paths { get; set; }

        public IEnumerable<Replacement> Replacements { get; set; }

        public IEnumerable<Module> Modules { get; set; }

        public bool IsVerbose { get; set; }

        public IEnumerable<ClassGenerationTag> Tags { get; set; }

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

        public void Sort(IEnumerable<Entity> sortedEntities)
        {
            Modules = sortedEntities
                .Select(entity => entity.Module)
                .Distinct();

            foreach (var module in Modules)
            {
                module.Sort(sortedEntities);
            }
        }
    }
}