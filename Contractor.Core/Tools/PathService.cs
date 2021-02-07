using Contractor.Core.Jobs;
using System.IO;

namespace Contractor.Core.Tools
{
    public class PathService
    {
        public string GetAbsolutePathForDbDomain(IDomainOptions options, string domainFolder)
        {
            string relativePath = domainFolder.Replace("{Domain}", options.Domain);
            string absolutePathForDomain = Path.Combine(options.DbDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForEntity(IEntityOptions options, string domainFolder)
        {
            string relativePath = options.ProjectName + domainFolder.Replace("{Domain}", options.Domain);
            relativePath = relativePath.Replace("{Entities}", options.EntityNamePlural);
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForDbContext(IDomainOptions options)
        {
            string relativePath = options.ProjectName + ".Persistence/Model/PersistenceDbContext.cs";
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForDTOs(IEntityOptions options, string domainFolder)
        {
            return Path.Combine(GetAbsolutePathForEntity(options, domainFolder), "DTOs");
        }

        public void AddDbDomainFolder(IDomainOptions options, string domainFolder)
        {
            string absolutePathForDbDomain = GetAbsolutePathForDbDomain(options, domainFolder);
            if (!Directory.Exists(absolutePathForDbDomain))
            {
                Directory.CreateDirectory(absolutePathForDbDomain);
            }
        }

        public void AddEntityFolder(IEntityOptions options, string domainFolder)
        {
            string absolutePathForDomain = GetAbsolutePathForEntity(options, domainFolder);
            if (!Directory.Exists(absolutePathForDomain))
            {
                Directory.CreateDirectory(absolutePathForDomain);
            }
        }

        public void AddDtoFolder(IEntityOptions options, string domainFolder)
        {
            string absolutePathForDTOs = GetAbsolutePathForDTOs(options, domainFolder);
            if (!Directory.Exists(absolutePathForDTOs))
            {
                Directory.CreateDirectory(absolutePathForDTOs);
            }
        }
    }
}