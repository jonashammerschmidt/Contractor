using Contractor.Core.Helpers;
using System.Collections.Generic;

namespace Contractor.Core
{
    public class Module
    {
        private string name;

        public IEnumerable<Entity> Entities { get; set; }

        public string Name
        {
            get { return name; }
            set { this.name = value.ToVariableName(); }
        }

        public string NameKebab
        {
            get { return StringConverter.PascalToKebabCase(Name); }
        }

        public bool Skip { get; set; }

        public ContractorGenerationOptions Options { get; private set; }

        public void AddLinks(ContractorGenerationOptions options)
        {
            Options = options;
        }

        public void AddLinksForChildren(ContractorGenerationOptions options)
        {
            foreach (var entity in Entities)
            {
                entity.AddLinks(this);
            }

            foreach (var entity in Entities)
            {
                entity.AddLinksForChildren(this);
            }
        }
    }
}