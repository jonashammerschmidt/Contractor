using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class Interface
    {
        public string Name { get; set; }
        
        public string Extends { get; set; }

        public List<InterfaceProperty> Properties { get; set; } = new();

        public List<InterfaceRelation> Relations { get; set; } = new();

        public List<Interface> ExtendedInterfaces { get; set; } = new();

        public void AddLinks(GenerationOptions generationOptions)
        {
            if (!string.IsNullOrWhiteSpace(this.Extends))
            {
                this.ExtendedInterfaces = this.Extends.Split(",")
                    .Select(extends => extends.Trim())
                    .Select(extends => generationOptions.Interfaces.Single(i => i.Name.ToLower() == extends.ToLower()))
                    .ToList();
            }
        }

        public Interface ToFlatInterface()
        {
            Interface flattenedInterface = new Interface();
            flattenedInterface.Properties.AddRange(this.Properties);
            flattenedInterface.Relations.AddRange(this.Relations);

            foreach (var extendedInterface in this.ExtendedInterfaces)
            {
                Interface flattenedSubInterfaces = extendedInterface.ToFlatInterface();
                flattenedInterface.Properties.AddRange(flattenedSubInterfaces.Properties);
                flattenedInterface.Relations.AddRange(flattenedSubInterfaces.Relations);
            }

            return flattenedInterface;
        }
    }
}