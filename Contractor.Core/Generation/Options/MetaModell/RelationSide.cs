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
                RelationSideType = RelationSideType.Target,
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
                RelationSideType = RelationSideType.Source,
                OtherEntity = entityFrom,
                OtherName = otherName,
                IsCreatedByPreProcessor = false,
                RelationBeforePreProcessor = null
            };
        }

        public static RelationSide FromObjectRelationEndFrom(Relation relation, string prefix, string postfix)
        {
            var is1ToN = relation.GetType() == typeof(Relation1ToN);
            string name = relation.PropertyNameInTarget ?? (is1ToN ? relation.SourceEntity.NamePlural : relation.SourceEntity.Name);
            string otherName = relation.PropertyNameInSource ?? relation.TargetEntity.Name;

            return new RelationSide()
            {
                Entity = relation.TargetEntity,
                IsDisplayProperty = false,
                IsOptional = true,
                OnDelete = relation.OnDelete,
                Name = name,
                Order = int.MaxValue,
                Type = prefix + relation.SourceEntity.Name + postfix,
                RelationSideType = RelationSideType.Target,
                OtherEntity = relation.SourceEntity,
                OtherName = otherName,
                IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor,
                RelationBeforePreProcessor = relation.RelationBeforePreProcessor
            };
        }

        public static RelationSide FromGuidRelationEndTo(Relation relation)
        {
            string name = relation.PropertyNameInSource ?? relation.TargetEntity.Name;
            string otherName = relation.PropertyNameInTarget ?? relation.SourceEntity.Name;
            name = name + "Id";
            otherName = otherName + "Id";

            return new RelationSide()
            {
                Entity = relation.SourceEntity,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                OnDelete = relation.OnDelete,
                Name = name,
                Order = int.MaxValue,
                Type = "Guid",
                RelationSideType = RelationSideType.Source,
                OtherEntity = relation.TargetEntity,
                OtherName = otherName,
                IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor,
                RelationBeforePreProcessor = relation.RelationBeforePreProcessor
            };
        }

        public static RelationSide FromObjectRelationEndTo(Relation relation, string prefix, string postfix)
        {
            var is1ToN = relation.GetType() == typeof(Relation1ToN);
            string name = relation.PropertyNameInSource ?? relation.TargetEntity.Name;
            string otherName = relation.PropertyNameInTarget ?? (is1ToN ? relation.SourceEntity.NamePlural : relation.SourceEntity.Name);

            return new RelationSide()
            {
                Entity = relation.SourceEntity,
                IsDisplayProperty = false,
                IsOptional = relation.IsOptional,
                OnDelete = relation.OnDelete,
                Name = name,
                Order = int.MaxValue,
                Type = prefix + relation.TargetEntity.Name + postfix,
                RelationSideType = RelationSideType.Source,
                OtherEntity = relation.TargetEntity,
                OtherName = otherName,
                IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor,
                RelationBeforePreProcessor = relation.RelationBeforePreProcessor
            };
        }
    }
}
