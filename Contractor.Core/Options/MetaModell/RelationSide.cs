using Contractor.Core.Helpers;

namespace Contractor.Core.MetaModell
{
    public class RelationSide : Property
    {
        public RelationSideType RelationSideType { get; private set; }

        public Entity OtherEntity { get; private set; }

        public string OnDelete { get; private set; }

        public bool IsCreatedByPreProcessor { get; set; }

        public Relation RelationBeforePreProcessor { get; set; }

        public string OtherName { get; private set; }

        public string OtherNameLower
        {
            get { return OtherName.LowerFirstChar(); }
        }

        public static RelationSide FromObjectRelationEndFrom(Entity entityFrom, Entity entityTo, string prefix, string postfix)
        {
            string name = entityTo.NamePlural;
            string otherName = entityFrom.Name;

            return new RelationSide()
            {
                Entity = entityFrom,
                IsDisplayProperty = false,
                IsOptional = false,
                OnDelete = "NoAction",
                Name = name,
                Order = int.MaxValue,
                Type = prefix + entityTo.Name + postfix,
                RelationSideType = RelationSideType.From,
                OtherEntity = entityTo,
                OtherName = otherName,
                IsCreatedByPreProcessor = false,
                RelationBeforePreProcessor = null
            };
        }

        public static RelationSide FromObjectRelationEndTo(Entity entityFrom, Entity entityTo, string prefix, string postfix)
        {
            string name = entityFrom.Name;
            string otherName = entityTo.NamePlural;

            return new RelationSide()
            {
                Entity = entityTo,
                IsDisplayProperty = false,
                IsOptional = false,
                OnDelete = "NoAction",
                Name = name,
                Order = int.MaxValue,
                Type = prefix + entityFrom.Name + postfix,
                RelationSideType = RelationSideType.To,
                OtherEntity = entityFrom,
                OtherName = otherName,
                IsCreatedByPreProcessor = false,
                RelationBeforePreProcessor = null
            };
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
                IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor,
                RelationBeforePreProcessor = relation.RelationBeforePreProcessor
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
                IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor,
                RelationBeforePreProcessor = relation.RelationBeforePreProcessor
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
                IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor,
                RelationBeforePreProcessor = relation.RelationBeforePreProcessor
            };
        }
    }
}