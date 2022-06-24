using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoDefaultPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            if (property.Type == PropertyType.String)
            {
                return $"    {property.Name.LowerFirstChar()}: '',";
            }

            return $"    {property.Name.LowerFirstChar()}: null,";
        }
    }
}