using Contractor.Core.MetaModell;
using Contractor.Core.Options;
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
                case PropertyTypes.String:
                    return $"        string {property.Name} {{ get; set; }}";

                case PropertyTypes.Integer:
                    return $"        int{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.Double:
                    return $"        double{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.DateTime:
                    return $"        DateTime{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.Boolean:
                    return $"        bool{optionalText} {property.Name} {{ get; set; }}";

                case PropertyTypes.Guid:
                    return $"        Guid{optionalText} {property.Name} {{ get; set; }}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}