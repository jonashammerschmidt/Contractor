using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class DatabaseEfDtoPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            string propertyLines = (property.IsOptional) ? "" : "[Required]\n";
            string optionalText = (property.IsOptional) ? "?" : "";
            switch (property.Type)
            {
                case PropertyTypes.String:
                    propertyLines +=
                        $"        [MaxLength({property.TypeExtra})]\n" +
                        $"        public string {property.Name} {{ get; set; }}";
                    break;

                case PropertyTypes.Double:
                    propertyLines +=
                        $"        public double{optionalText} {property.Name} {{ get; set; }}";
                    break;

                case PropertyTypes.Integer:
                    propertyLines +=
                        $"        public int{optionalText} {property.Name} {{ get; set; }}";
                    break;

                case PropertyTypes.DateTime:
                    propertyLines +=
                        $"        public DateTime{optionalText} {property.Name} {{ get; set; }}";
                    break;

                case PropertyTypes.Boolean:
                    propertyLines +=
                        $"        public bool{optionalText} {property.Name} {{ get; set; }}";
                    break;

                case PropertyTypes.Guid:
                    propertyLines +=
                        $"        public Guid{optionalText} {property.Name} {{ get; set; }}";
                    break;

                default:
                    throw new NotImplementedException();
            }

            return propertyLines;
        }
    }
}