using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoDefaultPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"    {options.PropertyName.LowerFirstChar()}: '',";

                case PropertyTypes.Integer:
                    return $"    {options.PropertyName.LowerFirstChar()}: 0,";

                case PropertyTypes.DateTime:
                    return $"    {options.PropertyName.LowerFirstChar()}: new Date(),";

                case PropertyTypes.Boolean:
                    return $"    {options.PropertyName.LowerFirstChar()}: false,";

                case PropertyTypes.Guid:
                    return $"    {options.PropertyName.LowerFirstChar()}: null,";

                default:
                    return $"    {options.PropertyName.LowerFirstChar()}: null,";
            }
        }
    }
}