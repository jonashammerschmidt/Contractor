using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                case PropertyTypes.Guid:
                    return $"    {options.PropertyName.LowerFirstChar()}: string;";

                case PropertyTypes.Integer:
                    return $"    {options.PropertyName.LowerFirstChar()}: number;";

                case PropertyTypes.DateTime:
                    return $"    {options.PropertyName.LowerFirstChar()}: Date;";

                case PropertyTypes.Boolean:
                    return $"    {options.PropertyName.LowerFirstChar()}: boolean;";

                default:
                    return $"    {options.PropertyName.LowerFirstChar()}: any;";
            }
        }
    }
}