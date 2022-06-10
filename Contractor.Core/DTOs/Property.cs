using Contractor.Core.Helpers;

namespace Contractor.Core
{
    public class Property
    {
        private string type;

        public string Name { get; set; }

        public string NameLower
        {
            get { return Name.LowerFirstChar(); }
        }

        public string Type {
            get { return this.type.Split(':')[0]; }
            set { this.type = value; }
        }

        public string TypeExtra
        {
            get { return this.type.Split(':')[1]; }
        }

        public bool IsOptional { get; set; }

        public bool IsDisplayProperty { get; set; }

        public int Order { get; set; }

        public Entity Entity { get; protected set; }

        public void AddLinks(Entity entity)
        {
            this.Entity = entity;
        }
    }
}