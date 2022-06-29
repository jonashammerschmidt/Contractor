using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendDtoPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            string optionalText = (property.IsOptional) ? "?" : "";
            switch (property.Type)
            {
                case PropertyType.ByteArray:
                case PropertyType.String:
                case PropertyType.Guid:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: string;";

                case PropertyType.Double:
                case PropertyType.Integer:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: number;";

                case PropertyType.DateTime:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: Date;";

                case PropertyType.Boolean:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: boolean;";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}