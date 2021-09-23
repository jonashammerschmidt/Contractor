using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database
{
    internal class DbTableAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbTableAddition(
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
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDbDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(IEntityAdditionOptions options, string templateFilePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(templateFilePath);
            fileData = fileData.Replace("RequestScopeDomain", options.RequestScopeDomain);
            fileData = fileData.Replace("RequestScopes", options.RequestScopeNamePlural);
            fileData = fileData.Replace("RequestScope", options.RequestScopeName);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("Domain", options.Domain);
            return fileData;
        }
    }
}