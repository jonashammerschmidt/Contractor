using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendEntityAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendEntityAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddEntity(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = fileSystemClient.ReadAllText(entity, templateFilePath);
            string filePath = GetFilePath(entity, domainFolder, templateFileName);

            fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Entity entity, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = pathService.GetAbsolutePathForFrontend(entity, domainFolder);
            string fileName = ModellNameReplacements.ReplaceEntityPlaceholders(entity, templateFileName);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }
    }
}