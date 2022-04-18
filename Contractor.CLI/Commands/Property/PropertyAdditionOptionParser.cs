using Contractor.CLI.Commands._Helper;
using Contractor.CLI.Tools;
using Contractor.Core.Options;
using System;

namespace Contractor.CLI
{
    internal class PropertyAdditionOptionParser
    {
        public static IPropertyAdditionOptions ParseOptions(IContractorOptions contractorOptions, string[] args)
        {
            IPropertyAdditionOptions options = new PropertyAdditionOptions(contractorOptions);

            options.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            options.PropertyType = ParsePropertyType(args);
            if (args[2].Contains(':'))
            {
                options.PropertyTypeExtra = args[2].Split(':')[1];
            }

            options.PropertyName = args[3];

            string entityName = ArgumentParser.ExtractArgument(args, "-e", "--entity");
            options.Domain = entityName.Split('.')[0];
            options.EntityName = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePlural = entityName.Split(':')[1];

            options.IsOptional = ArgumentParser.HasArgument(args, "-o", "--optional");

            TagArgumentParser.AddTags(args, options);

            return options;
        }

        private static PropertyTypes ParsePropertyType(string[] args)
        {
            string propertyType = args[2].Split(':')[0].Trim();
            switch (propertyType.ToLower())
            {
                case "string":
                case "varchar":
                case "nvarchar":
                    return PropertyTypes.String;

                case "short":
                case "int":
                case "integer":
                case "long":
                    return PropertyTypes.Integer;

                case "datetime":
                case "date":
                case "time":
                    return PropertyTypes.DateTime;

                case "bit":
                case "bool":
                case "boolean":
                    return PropertyTypes.Boolean;

                case "double":
                case "float":
                case "number":
                case "decimal":
                    return PropertyTypes.Double;

                case "guid":
                    return PropertyTypes.Guid;

                default:
                    throw new ArgumentException("PropertyType cannot be parsed: " + propertyType);
            }
        }
    }
}