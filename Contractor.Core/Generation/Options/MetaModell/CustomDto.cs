using System.Collections.Generic;

namespace Contractor.Core.MetaModell
{
    public class PurposeDto
    {
        public string EntityName { get; set; }
        
        public string Purpose { get; set; }

        public List<PurposeDtoProperty> Properties { get; set; }

        public Entity Entity { get; set; }
        
        public void AddLinks(GenerationOptions options)
        {
            this.Entity = options.FindEntity(EntityName);
            foreach (var purposeDtoProperty in Properties)
            {
                purposeDtoProperty.AddLinks(options, this.Entity);
            }
        }
    }
}