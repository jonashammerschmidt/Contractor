using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    internal static class CSharpProperties
    {
        public static string ToString(PropertyTypes propertyType) {
            switch (propertyType)
            {
                case PropertyTypes.String:
                    return "string";
                case PropertyTypes.Integer:
                    return "integer";
                case PropertyTypes.Float:
                    return "float";
                case PropertyTypes.DateTime:
                    return "DateTime";
                case PropertyTypes.Boolean:
                    return "bool";
                case PropertyTypes.Guid:
                    return "Guid";
                default:
                    return "object";
            }
        }
    }
}