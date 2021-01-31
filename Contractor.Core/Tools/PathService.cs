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

        public string GetAbsolutePathForDomain(IDomainOptions options, string domainFolder)
        {
            string relativePath = options.ProjectName + domainFolder.Replace("{Domain}", options.Domain);
            string absolutePathForDomain = Path.Combine(options.BackendDestinationFolder, relativePath);
            return absolutePathForDomain;
        }

        public string GetAbsolutePathForDTOs(IDomainOptions options, string domainFolder)
        {
            return Path.Combine(GetAbsolutePathForDomain(options, domainFolder), "DTOs");
        }

        public void AddDbDomainFolder(IDomainOptions options, string domainFolder)
        {
            string absolutePathForDbDomain = GetAbsolutePathForDbDomain(options, domainFolder);
            if (!Directory.Exists(absolutePathForDbDomain))
            {
                Directory.CreateDirectory(absolutePathForDbDomain);
            }
        }

        public void AddDomainFolder(IDomainOptions options, string domainFolder)
        {
            string absolutePathForDomain = GetAbsolutePathForDomain(options, domainFolder);
            if (!Directory.Exists(absolutePathForDomain))
            {
                Directory.CreateDirectory(absolutePathForDomain);
            }
        }

        public void AddDtoFolder(IDomainOptions options, string domainFolder)
        {
            string absolutePathForDTOs = GetAbsolutePathForDTOs(options, domainFolder);
            if (!Directory.Exists(absolutePathForDTOs))
            {
                Directory.CreateDirectory(absolutePathForDTOs);
            }
        }

        public void DeleteDomainFolder(IDomainOptions options, string domainFolder)
        {
            string absolutePathForDomain = GetAbsolutePathForDomain(options, domainFolder);
            if (Directory.Exists(absolutePathForDomain))
            {
                Directory.Delete(absolutePathForDomain, true);
            }
        }
    }
}