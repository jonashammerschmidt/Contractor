using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            string optionalText = (options.IsOptional) ? "?" : "";
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                case PropertyTypes.Guid:
                    return $"    {options.PropertyName.LowerFirstChar()}{optionalText}: string;";

                case PropertyTypes.Double:
                case PropertyTypes.Integer:
                    return $"    {options.PropertyName.LowerFirstChar()}{optionalText}: number;";

                case PropertyTypes.DateTime:
                    return $"    {options.PropertyName.LowerFirstChar()}{optionalText}: Date;";

                case PropertyTypes.Boolean:
                    return $"    {options.PropertyName.LowerFirstChar()}{optionalText}: boolean;";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}