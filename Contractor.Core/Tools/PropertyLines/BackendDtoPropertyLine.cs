using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendDtoPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            string optionalText = (options.IsOptional) ? "?" : "";
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"        public string {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Double:
                    return $"        public double{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Integer:
                    return $"        public int{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.DateTime:
                    return $"        public DateTime{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Boolean:
                    return $"        public bool{optionalText} {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Guid:
                    return $"        public Guid{optionalText} {options.PropertyName} {{ get; set; }}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}