using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class PathService
    {
        public string GetAbsolutePathForDbDomain(IDomainAdditionOptions options, string domainFolder)
        {
            string relativePath = domainFolder.Replace("{Domain}", options.Domain);
            string absolutePathForDomain = Path.Combine(options.DbDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForEntity(IEntityAdditionOptions options, string domainFolder)
        {
            string relativePath = domainFolder;
            relativePath = relativePath.Replace("{Domain}", options.Domain);
            relativePath = relativePath.Replace("{Entities}", options.EntityNamePlural);
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForFrontendModel(IEntityAdditionOptions options, string domainFolder)
        {
            string relativePath = domainFolder;
            relativePath = relativePath.Replace("{domain-kebab}", StringConverter.PascalToKebabCase(options.Domain));
            relativePath = relativePath.Replace("{entity-kebab}", StringConverter.PascalToKebabCase(options.EntityName));
            relativePath = relativePath.Replace("{entities-kebab}", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            string absolutePathForDomain = Path.Combine(options.FrontendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForFrontendPages(IEntityAdditionOptions options, string domainFolder)
        {
            string relativePath = domainFolder;
            relativePath = relativePath.Replace("{domain-kebab}", StringConverter.PascalToKebabCase(options.Domain));
            relativePath = relativePath.Replace("{entity-kebab}", StringConverter.PascalToKebabCase(options.EntityName));
            relativePath = relativePath.Replace("{entities-kebab}", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            string absolutePathForDomain = Path.Combine(options.FrontendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForDbContext(IContractorOptions options)
        {
            string relativePath = "Persistence\\PersistenceDbContext.cs";
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForInMemoryDbContext(IContractorOptions options)
        {
            string relativePath = "Persistence.Tests\\InMemoryDbContext.cs";
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForDTOs(IEntityAdditionOptions options, string domainFolder)
        {
            return Path.Combine(GetAbsolutePathForEntity(options, domainFolder), "DTOs");
        }

        public void AddDbDomainFolder(IDomainAdditionOptions options, string domainFolder)
        {
            string absolutePathForDbDomain = GetAbsolutePathForDbDomain(options, domainFolder);
            if (!Directory.Exists(absolutePathForDbDomain))
            {
                Directory.CreateDirectory(absolutePathForDbDomain);
            }
        }

        public void AddEntityFolder(IEntityAdditionOptions options, string domainFolder)
        {
            string absolutePathForDomain = GetAbsolutePathForEntity(options, domainFolder);
            if (!Directory.Exists(absolutePathForDomain))
            {
                Directory.CreateDirectory(absolutePathForDomain);
            }
        }

        public void AddDtoFolder(IEntityAdditionOptions options, string domainFolder)
        {
            string absolutePathForDTOs = GetAbsolutePathForDTOs(options, domainFolder);
            if (!Directory.Exists(absolutePathForDTOs))
            {
                Directory.CreateDirectory(absolutePathForDTOs);
            }
        }
    }
}