using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class DatabaseTablePropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            string spaces = " ";
            int spaceCount = 20 - options.PropertyName.Length;
            for (int i = 0; i < spaceCount; i++)
            {
                spaces += " ";
            }

            string nullableText = (options.IsOptional) ? "NULL" : "NOT NULL";
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"	[{options.PropertyName}]{spaces}NVARCHAR ({options.PropertyTypeExtra})   {nullableText},";

                case PropertyTypes.Double:
                    return $"	[{options.PropertyName}]{spaces}FLOAT            {nullableText},";

                case PropertyTypes.Integer:
                    return $"	[{options.PropertyName}]{spaces}INT              {nullableText},";

                case PropertyTypes.Guid:
                    return $"	[{options.PropertyName}]{spaces}UNIQUEIDENTIFIER {nullableText},";

                case PropertyTypes.Boolean:
                    return $"	[{options.PropertyName}]{spaces}BIT              {nullableText},";

                case PropertyTypes.DateTime:
                    return $"	[{options.PropertyName}]{spaces}DATETIME         {nullableText},";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}