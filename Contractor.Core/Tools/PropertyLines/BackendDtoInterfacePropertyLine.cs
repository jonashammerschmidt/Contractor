using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class BackendDtoInterfacePropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"        string {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Integer:
                    return $"        int {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.DateTime:
                    return $"        DateTime {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Boolean:
                    return $"        bool {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Guid:
                    return $"        Guid {options.PropertyName} {{ get; set; }}";

                default:
                    return $"        object {options.PropertyName} {{ get; set; }}";
            }
        }
    }
}