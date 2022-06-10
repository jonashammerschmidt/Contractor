using System.IO;

namespace Contractor.Core.Tools
{
    internal class UsingStatementAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public UsingStatementAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Entity entity, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            string filePath = GetFilePath(entity, domainFolder, templateFileName);
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            fileData = UsingStatements.Add(fileData, namespaceToAdd);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Entity entity, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForBackend(entity, domainFolder);
            string fileName = templateFileName.Replace("Entities", entity.NamePlural);
            fileName = fileName.Replace("Entity", entity.Name);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }
    }
}