using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoDefaultPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            if (options.IsOptional && options.PropertyType != PropertyTypes.String)
            {
                return $"    {options.PropertyName.LowerFirstChar()}: '',";
            }

            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"    {options.PropertyName.LowerFirstChar()}: '',";

                case PropertyTypes.Integer:
                    return $"    {options.PropertyName.LowerFirstChar()}: 0,";

                case PropertyTypes.Double:
                    return $"    {options.PropertyName.LowerFirstChar()}: 0.0,";

                case PropertyTypes.DateTime when !options.IsOptional:
                    return $"    {options.PropertyName.LowerFirstChar()}: new Date(),";

                case PropertyTypes.Boolean:
                case PropertyTypes.Guid:
                    return $"    {options.PropertyName.LowerFirstChar()}: null,";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}