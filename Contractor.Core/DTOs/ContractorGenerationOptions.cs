using Contractor.Core;
using System.Collections.Generic;

namespace Contractor.Core
{
    public class ContractorGenerationOptions
    {
        public Paths Paths { get; set; }

        public IEnumerable<Replacement> Replacements { get; set; }

        public IEnumerable<Module> Modules { get; set; }

        public void AddLinks()
        {
            foreach (var module in this.Modules)
            {
                module.AddLinks(this);
            }
        }

        public Entity FindEntity(string entityName)
        {
            foreach (var module in this.Modules)
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
    }
}