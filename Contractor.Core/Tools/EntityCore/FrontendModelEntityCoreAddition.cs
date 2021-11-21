using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendModelEntityCoreAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendModelEntityCoreAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddEntityCore(IEntityAdditionOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = GetFileData(options, templateFilePath);
            string filePath = GetFilePath(options, domainFolder, templateFileName);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontend(options, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(IEntityAdditionOptions options, string templateFilePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(templateFilePath);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            fileData = fileData.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            fileData = fileData.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileData = fileData.Replace("RequestScopeDomain", options.RequestScopeDomain);
            fileData = fileData.Replace("RequestScopes", options.RequestScopeNamePlural);
            fileData = fileData.Replace("RequestScope", options.RequestScopeName);
            fileData = fileData.Replace("Domain", options.Domain);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);

            return fileData;
        }
    }
}