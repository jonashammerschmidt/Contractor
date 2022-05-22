using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class PathService
    {
        public string GetAbsolutePathForBackend(IContractorOptions options, string domainFolder)
        {
            return Path.Combine(options.BackendDestinationFolder, domainFolder);
        }

        public string GetAbsolutePathForBackend(IEntityAdditionOptions options, string domainFolder)
        {
            string absolutePath = GetAbsolutePathForBackend(options as IContractorOptions, domainFolder);
            absolutePath = absolutePath.Replace("Domain", options.Domain);
            absolutePath = absolutePath.Replace("Entities", options.EntityNamePlural);

            return absolutePath;
        }

        public string GetAbsolutePathForDatabase(IContractorOptions options, string domainFolder)
        {
            return Path.Combine(options.DbDestinationFolder, domainFolder);
        }

        public string GetAbsolutePathForDatabase(IEntityAdditionOptions options, string domainFolder)
        {
            string absolutePath = GetAbsolutePathForDatabase(options as IContractorOptions, domainFolder);
            absolutePath = absolutePath.Replace("Domain", options.Domain);
            absolutePath = absolutePath.Replace("Entities", options.EntityNamePlural);
            return absolutePath;
        }

        public string GetAbsolutePathForFrontend(IDomainAdditionOptions options, string domainFolder)
        {
            string absolutePath = Path.Combine(options.FrontendDestinationFolder, domainFolder);
            absolutePath = absolutePath.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            return absolutePath;
        }

        public string GetAbsolutePathForFrontend(IEntityAdditionOptions options, string domainFolder)
        {
            string absolutePath = GetAbsolutePathForFrontend(options as IDomainAdditionOptions, domainFolder);
            absolutePath = absolutePath.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            absolutePath = absolutePath.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            return absolutePath;
        }
    }
}