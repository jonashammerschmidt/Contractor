using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendDtoInterfacePropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            string optionalText = (property.IsOptional) ? "?" : "";
            switch (property.Type)
            {
                case PropertyType.String:
                    return $"        string {property.Name} {{ get; set; }}";

                case PropertyType.Integer:
                    return $"        int{optionalText} {property.Name} {{ get; set; }}";

                case PropertyType.Double:
                    return $"        double{optionalText} {property.Name} {{ get; set; }}";

                case PropertyType.DateTime:
                    return $"        DateTime{optionalText} {property.Name} {{ get; set; }}";

                case PropertyType.Boolean:
                    return $"        bool{optionalText} {property.Name} {{ get; set; }}";

                case PropertyType.Guid:
                    return $"        Guid{optionalText} {property.Name} {{ get; set; }}";

                case PropertyType.ByteArray:
                    return $"        byte[] {property.Name} {{ get; set; }}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}