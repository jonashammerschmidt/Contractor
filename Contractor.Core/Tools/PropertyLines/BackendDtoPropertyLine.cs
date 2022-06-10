using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendDtoPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            string optionalText = (property.IsOptional) ? "?" : "";
            switch (property.Type)
            {
                case PropertyTypes.String:
                    return $"        public string {property.Name} {{ get; set; }}";

                case PropertyTypes.Double:
                    return $"        public double{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.Integer:
                    return $"        public int{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.DateTime:
                    return $"        public DateTime{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.Boolean:
                    return $"        public bool{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.Guid:
                    return $"        public Guid{optionalText} {property.Name} {{ get; set; }}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}