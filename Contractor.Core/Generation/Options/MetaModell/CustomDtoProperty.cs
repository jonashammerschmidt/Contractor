using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class PurposeDtoProperty
    {
        public string Path { get; set; }

        public List<PurposeDtoPathItem> PathItems { get; set; }

        public void AddLinks(GenerationOptions options, Entity entity)
        {
            var currentEntity = entity;
            this.PathItems = this.Path
                .Split('.', '<')
                .Select(pathItem =>
                {
                    var relation = options.FindRelation(currentEntity, pathItem);
                    var otherEntity = relation.EntityFrom == currentEntity ? relation.EntityTo : relation.EntityFrom;
                    var purposeDtoPathItem = new PurposeDtoPathItem()
                    {
                        PropertyName = pathItem,
                        Entity = currentEntity,
                        OtherEntity = otherEntity,
                        Relation = relation,
                    };
                    currentEntity = otherEntity;
                    return purposeDtoPathItem;
                })
                .ToList();
        }
    }
}