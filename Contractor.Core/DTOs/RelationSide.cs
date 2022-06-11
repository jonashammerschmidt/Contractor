using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core
{
    public class RelationSide : Property
    {
        public RelationSideType RelationSideType { get; private set; }

        public Entity OtherEntity { get; private set; }

        public string OtherName { get; private set; }

        public string OtherNameLower
        {
            get { return OtherName.LowerFirstChar(); }
        }

        public static RelationSide FromObjectRelationEndFrom(Relation relation, string prefix, string postfix)
        {
            return new RelationSide()
            {
                Entity = relation.EntityFrom,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                Name = relation.PropertyNameTo,
                Order = int.MaxValue,
                Type = prefix + relation.EntityTo.Name + postfix,
                RelationSideType = RelationSideType.From,
                OtherEntity = relation.EntityTo,
                OtherName = relation.PropertyNameTo,
            };
        }

        public static RelationSide FromGuidRelationEndTo(Relation relation)
        {
            return new RelationSide()
            {
                Entity = relation.EntityTo,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                Name = relation.PropertyNameFrom + "Id",
                Order = int.MaxValue,
                Type = "Guid",
                RelationSideType = RelationSideType.To,
                OtherEntity = relation.EntityFrom,
                OtherName = relation.PropertyNameFrom + "Id",
            };
        }

        public static RelationSide FromObjectRelationEndTo(Relation relation, string prefix, string postfix)
        {
            return new RelationSide()
            {
                Entity = relation.EntityTo,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                Name = relation.PropertyNameFrom,
                Order = int.MaxValue,
                Type = prefix + relation.EntityFrom.Name + postfix,
                RelationSideType = RelationSideType.To,
                OtherEntity = relation.EntityFrom,
                OtherName = relation.PropertyNameFrom,
            };
        }
    }
}