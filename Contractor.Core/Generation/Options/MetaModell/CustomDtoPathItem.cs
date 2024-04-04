namespace Contractor.Core.MetaModell
{
    public class PurposeDtoPathItem
    {
        public Entity Entity { get; set; }
        
        public Entity OtherEntity { get; set; }

        public Relation Relation { get; set; }

        public string PropertyName { get; set; }

        public override string ToString()
        {
            return Entity.Name + "." + PropertyName;
        }
    }
}