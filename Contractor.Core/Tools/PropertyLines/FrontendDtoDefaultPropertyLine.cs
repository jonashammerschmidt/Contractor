using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoDefaultPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            if (options.PropertyType == PropertyTypes.String)
            {
                return $"    {options.PropertyName.LowerFirstChar()}: '',";
            }

            return $"    {options.PropertyName.LowerFirstChar()}: null,";
        }
    }
}