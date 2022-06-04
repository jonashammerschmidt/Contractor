using System.Collections.Generic;

namespace Contractor.CLI.DTOs
{
    public class Module
    {
        public IEnumerable<Entity> Entities { get; set; }

        public string Name { get; set; }

        public ContractorGenerationOptions Options { get; private set; }

        public void AddLinks(ContractorGenerationOptions options)
        {
            this.Options = options;

            foreach (var entity in this.Entities)
            {
                entity.AddLinks(this);
            }
        }
    }
}