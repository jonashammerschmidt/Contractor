using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoDefaultPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            if (property.Type == PropertyTypes.String)
            {
                return $"    {property.Name.LowerFirstChar()}: '',";
            }

            return $"    {property.Name.LowerFirstChar()}: null,";
        }
    }
}