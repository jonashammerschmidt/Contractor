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
                    return "int";
                case PropertyTypes.Double:
                    return "double";
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