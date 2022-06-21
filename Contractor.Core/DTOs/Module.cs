using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

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

        public string NameReadable
        {
            get { return Name.ToReadable(); }
        }

        public bool Skip { get; set; }

        public ContractorGenerationOptions Options { get; private set; }

        public void AddLinks(ContractorGenerationOptions options)
        {
            Options = options;
        }

        public void AddLinksForChildren()
        {
            foreach (var entity in Entities)
            {
                entity.AddLinks(this);
            }

            foreach (var entity in Entities)
            {
                entity.AddLinksForChildren();
            }
        }

        public void Sort(IEnumerable<Entity> sortedEntities)
        {
            this.Entities = sortedEntities
                .Where(entity => entity.Module == this);
        }
    }
}