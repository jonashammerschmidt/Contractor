using Contractor.Core.Helpers;

namespace Contractor.Core.MetaModell
{
    public class Property
    {
        private string type;
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value.ToVariableName(); }
        }

        public string NameLower
        {
            get { return Name.LowerFirstChar(); }
        }

        public string Type
        {
            get { return type.Split(':')[0]; }
            set { type = value; }
        }

        public string TypeExtra
        {
            get { return type.Split(':')[1]; }
        }

        public bool IsOptional { get; set; }

        public bool IsDisplayProperty { get; set; }

        public bool IsHidden { get; set; }

        public int Order { get; set; }

        public Entity Entity { get; protected set; }

        public void AddLinks(Entity entity)
        {
            Entity = entity;
        }
    }
}