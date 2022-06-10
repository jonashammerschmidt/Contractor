namespace Contractor.Core.Tools
{
    internal static class CSharpProperties
    {
        public static string ToString(string type)
        {
            switch (type)
            {
                case "Boolean":
                    return "bool";

                case "DateTime":
                    return "DateTime";

                case "Double":
                    return "double";

                case "Guid":
                    return "Guid";

                case "Integer":
                    return "int";

                case "String":
                    return "string";

                default:
                    return "object";
            }
        }
    }
}