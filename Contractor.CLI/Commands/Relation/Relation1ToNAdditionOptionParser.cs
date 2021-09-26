using Contractor.CLI.Tools;
using Contractor.Core.Options;

namespace Contractor.CLI
{
    internal class Relation1ToNAdditionOptionParser
    {
        public static IRelationAdditionOptions ParseOptions(IContractorOptions contractorOptions, string[] args)
        {
            RelationAdditionOptions options = new RelationAdditionOptions(contractorOptions);

            options.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            string entityName = args[3];
            options.DomainFrom = entityName.Split('.')[0];
            options.EntityNameFrom = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePluralFrom = entityName.Split(':')[1];

            entityName = args[4];
            options.DomainTo = entityName.Split('.')[0];
            options.EntityNameTo = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePluralTo = entityName.Split(':')[1];

            options.IsOptional = ArgumentParser.HasArgument(args, "-o", "--optional");

            if (ArgumentParser.HasArgument(args, "-n", "--alternative-property-names"))
            {
                string st = ArgumentParser.ExtractArgument(args, "-n", "--alternative-property-names");
                options.PropertyNameFrom = st.Split(':')[0];
                options.PropertyNameTo = st.Split(':')[1];
            }
            else
            {
                options.PropertyNameFrom = options.EntityNameFrom;
                options.PropertyNameTo = options.EntityNamePluralTo;
            }

            return options;
        }
    }
};