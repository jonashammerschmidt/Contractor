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
            string relativePath = options.ProjectName + domainFolder;
            relativePath = relativePath.Replace("{Domain}", options.Domain);
            relativePath = relativePath.Replace("{Entities}", options.EntityNamePlural);
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForDbContext(IContractorOptions options)
        {
            string relativePath = options.ProjectName + ".Persistence\\Model\\PersistenceDbContext.cs";
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForInMemoryDbContext(IContractorOptions options)
        {
            string relativePath = options.ProjectName + ".Persistence.Tests\\Model\\InMemoryDbContext.cs";
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