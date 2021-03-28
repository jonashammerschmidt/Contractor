using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class BackendDtoPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"        public string {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Integer:
                    return $"        public int {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.DateTime:
                    return $"        public DateTime {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Boolean:
                    return $"        public bool {options.PropertyName} {{ get; set; }}";

                case PropertyTypes.Guid:
                    return $"        public Guid {options.PropertyName} {{ get; set; }}";

                default:
                    return $"        public object {options.PropertyName} {{ get; set; }}";
            }
        }
    }
}