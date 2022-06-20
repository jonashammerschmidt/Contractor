using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core
{
    public class RelationSide : Property
    {
        public RelationSideType RelationSideType { get; private set; }

        public Entity OtherEntity { get; private set; }

        public string OnDelete { get; private set; }

        public string OtherName { get; private set; }

        public string OtherNameLower
        {
            get { return OtherName.LowerFirstChar(); }
        }

        public static RelationSide FromObjectRelationEndFrom(Relation relation, string prefix, string postfix)
        {
            var is1ToN = relation.GetType() == typeof(Relation1ToN);
            string name = relation.PropertyNameTo ?? (is1ToN ? relation.EntityTo.NamePlural : relation.EntityTo.Name);
            string otherName = relation.PropertyNameFrom ?? relation.EntityFrom.Name;

            return new RelationSide()
            {
                Entity = relation.EntityFrom,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                OnDelete = relation.OnDelete,
                Name = name,
                Order = int.MaxValue,
                Type = prefix + relation.EntityTo.Name + postfix,
                RelationSideType = RelationSideType.From,
                OtherEntity = relation.EntityTo,
                OtherName = otherName,
            };
        }

        public static RelationSide FromGuidRelationEndTo(Relation relation)
        {
            string name = relation.PropertyNameFrom ?? relation.EntityFrom.Name;
            string otherName = relation.PropertyNameTo ?? relation.EntityTo.Name;
            name = name + "Id";
            otherName = otherName + "Id";

            return new RelationSide()
            {
                Entity = relation.EntityTo,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                OnDelete = relation.OnDelete,
                Name = name,
                Order = int.MaxValue,
                Type = "Guid",
                RelationSideType = RelationSideType.To,
                OtherEntity = relation.EntityFrom,
                OtherName = otherName,
            };
        }

        public static RelationSide FromObjectRelationEndTo(Relation relation, string prefix, string postfix)
        {
            var is1ToN = relation.GetType() == typeof(Relation1ToN);
            string name = relation.PropertyNameFrom ?? relation.EntityFrom.Name;
            string otherName = relation.PropertyNameTo ?? (is1ToN ? relation.EntityTo.NamePlural : relation.EntityTo.Name);

            return new RelationSide()
            {
                Entity = relation.EntityTo,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                OnDelete = relation.OnDelete,
                Name = name,
                Order = int.MaxValue,
                Type = prefix + relation.EntityFrom.Name + postfix,
                RelationSideType = RelationSideType.To,
                OtherEntity = relation.EntityFrom,
                OtherName = otherName,
            };
        }
    }
}