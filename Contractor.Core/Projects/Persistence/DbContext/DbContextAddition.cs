using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    public class DbContextAddition
    {
        public PathService pathService;

        public DbContextAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(DomainOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = GetFileData(options, templateFilePath);
            string filePath = GetFilePath(options, domainFolder, templateFileName);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(DomainOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Domain", options.Domain);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(DomainOptions options, string templateFilePath)
        {
            string fileData = File.ReadAllText(templateFilePath);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("Domain", options.Domain);
            return fileData;
        }
    }
}