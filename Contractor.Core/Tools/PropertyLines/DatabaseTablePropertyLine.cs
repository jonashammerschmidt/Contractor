using Contractor.Core.Options;

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

            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return $"	[{options.PropertyName}]{spaces}NVARCHAR ({options.PropertyTypeExtra})   NOT NULL,";

                case PropertyTypes.Float:
                    return $"	[{options.PropertyName}]{spaces}FLOAT            NOT NULL,";

                case PropertyTypes.Integer:
                    return $"	[{options.PropertyName}]{spaces}INT              NOT NULL,";

                case PropertyTypes.Guid:
                    return $"	[{options.PropertyName}]{spaces}UNIQUEIDENTIFIER NOT NULL,";

                case PropertyTypes.Boolean:
                    return $"	[{options.PropertyName}]{spaces}BIT              NOT NULL,";

                case PropertyTypes.DateTime:
                    return $"	[{options.PropertyName}]{spaces}DATETIME         NOT NULL,";

                default:
                    return $"-- TODO: {options.PropertyType} {options.PropertyName}";
            }
        }
    }
}