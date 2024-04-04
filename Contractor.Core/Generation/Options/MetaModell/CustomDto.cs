using System.Collections.Generic;

namespace Contractor.Core.MetaModell
{
    public class CustomDto
    {
        public string EntityName { get; set; }
        
        public string Purpose { get; set; }

        public List<CustomDtoProperty> Properties { get; set; }

        public Entity Entity { get; set; }
        
        public void AddLinks(GenerationOptions options)
        {
            this.Entity = options.FindEntity(EntityName);
            foreach (var customDtoProperty in Properties)
            {
                customDtoProperty.AddLinks(options, this.Entity);
            }
        }
    }
}