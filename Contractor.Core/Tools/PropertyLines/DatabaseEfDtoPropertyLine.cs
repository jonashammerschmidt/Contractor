using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class DatabaseEfDtoPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            string optionalText = (property.IsOptional) ? "?" : "";
            switch (property.Type)
            {
                case PropertyType.String:
                    return $"        public string {property.Name} {{ get; set; }}";
                case PropertyType.Double:
                    return $"        public double{optionalText} {property.Name} {{ get; set; }}";
                case PropertyType.Integer:
                    return $"        public int{optionalText} {property.Name} {{ get; set; }}";
                case PropertyType.DateTime:
                    return $"        public DateTime{optionalText} {property.Name} {{ get; set; }}";
                case PropertyType.Boolean:
                    return $"        public bool{optionalText} {property.Name} {{ get; set; }}";
                case PropertyType.Guid:
                    return $"        public Guid{optionalText} {property.Name} {{ get; set; }}";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}