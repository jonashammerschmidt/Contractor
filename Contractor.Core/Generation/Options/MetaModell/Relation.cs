using Contractor.Core.Helpers;

namespace Contractor.Core.MetaModell
{
    public abstract class Relation
    {
        private string targetEntityName;
        protected string propertyNameInSource;
        private string propertyNameInTarget;

        protected Relation()
        {
        }

        protected Relation(Relation relation)
        {
            targetEntityName = relation.targetEntityName;
            propertyNameInSource = relation.propertyNameInSource;
            propertyNameInTarget = relation.propertyNameInTarget;

            IsOptional = relation.IsOptional;
            OnDelete = relation.OnDelete;
            IsCreatedByPreProcessor = relation.IsCreatedByPreProcessor;
            RelationBeforePreProcessor = relation.RelationBeforePreProcessor;
            Order = relation.Order;
            TargetEntity = relation.TargetEntity;
            SourceEntity = relation.SourceEntity;
        }

        public string TargetEntityName
        {
            set { targetEntityName = value.ToVariableName(); }
        }

        public string PropertyNameInSource
        {
            get { return propertyNameInSource; }
            set { propertyNameInSource = value?.ToVariableName(); }
        }

        public string PropertyNameInTarget
        {
            get { return propertyNameInTarget; }
            set { propertyNameInTarget = value?.ToVariableName(); }
        }

        public bool IsOptional { get; set; }

        public int Order { get; set; }

        public bool IsCreatedByPreProcessor { get; internal set; }

        public Relation RelationBeforePreProcessor { get; internal set; }

        public string OnDelete { get; set; }

        public Entity TargetEntity { get; set; }

        public Entity SourceEntity { get; set; }

        public void AddLinks(Entity entity)
        {
            SourceEntity = entity;
            TargetEntity = entity.Module.Options.FindEntity(targetEntityName);
        }
    }
}