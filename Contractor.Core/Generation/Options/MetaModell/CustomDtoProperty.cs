using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class CustomDtoProperty
    {
        public string Path { get; set; }

        public List<CustomDtoPathItem> PathItems { get; set; }

        public void AddLinks(GenerationOptions options, Entity entity)
        {
            var currentEntity = entity;
            this.PathItems = this.Path
                .Split('.', '<')
                .Select(pathItem =>
                {
                    var relation = options.FindRelation(currentEntity, pathItem);
                    var customDtoPathItem = new CustomDtoPathItem()
                    {
                        PropertyName = pathItem,
                        Entity = currentEntity,
                        Relation = relation,
                    };
                    currentEntity = relation.EntityFrom == currentEntity ? relation.EntityTo : relation.EntityFrom;
                    return customDtoPathItem;
                })
                .ToList();
        }
    }
}