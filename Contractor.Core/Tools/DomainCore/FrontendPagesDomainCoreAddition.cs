using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendPagesDomainCoreAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendPagesDomainCoreAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddEntityCore(Module module, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = GetFileData(module, templateFilePath);
            string filePath = GetFilePath(module, domainFolder, templateFileName);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Module module, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontend(module, domainFolder);
            string fileName = templateFileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(module.Name));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(Module module, string templateFilePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(templateFilePath);
            fileData = fileData.Replace("domain-kebab", StringConverter.PascalToKebabCase(module.Name));
            fileData = fileData.Replace("Domain", module.Name);

            return fileData;
        }
    }
}