using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendEfDtoInterfacePropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            string optionalText = (options.IsOptional) ? "?" : "";
            switch (options.PropertyType)
            {
                case PropertyTypes.String when int.Parse(options.PropertyTypeExtra) <= 1000:
                    return $"        string {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.String when int.Parse(options.PropertyTypeExtra) > 1000:
                    return $"        byte[] {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Integer:
                    return $"        int{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Double:
                    return $"        double{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.DateTime:
                    return $"        DateTime{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Boolean:
                    return $"        bool{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Guid:
                    return $"        Guid{optionalText} {options.PropertyName} {{ get; set; }}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}