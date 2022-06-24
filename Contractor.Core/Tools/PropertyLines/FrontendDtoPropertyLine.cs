using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Options;
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
                case PropertyTypes.String:
                case PropertyTypes.Guid:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: string;";

                case PropertyTypes.Double:
                case PropertyTypes.Integer:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: number;";

                case PropertyTypes.DateTime:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: Date;";

                case PropertyTypes.Boolean:
                    return $"    {property.Name.LowerFirstChar()}{optionalText}: boolean;";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}