using Contractor.CLI.Tools;

namespace Contractor.CLI.Migration
{
    public class EntityAdditionOptionParser
    {
        public static EntityAdditionOptions ParseOptions(IContractorOptions contractorOptions, string[] args)
        {
            EntityAdditionOptions options = new EntityAdditionOptions(contractorOptions);

            options.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            string entityName = args[2];
            options.Domain = entityName.Split('.')[0];
            options.EntityName = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePlural = entityName.Split(':')[1];

            if (ArgumentParser.HasArgument(args, "-s", "--scope"))
            {
                string st = ArgumentParser.ExtractArgument(args, "-s", "--scope");
                options.RequestScopeDomain = st.Split(':')[0].Split('.')[0];
                options.RequestScopeName = st.Split(':')[0].Split('.')[1];
                options.RequestScopeNamePlural = st.Split(':')[1];
            }

            return options;
        }
    }
};