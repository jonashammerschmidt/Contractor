using System.IO;

namespace Contractor.Core.Tools
{
    internal class EntityCoreAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityCoreAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddEntityCore(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, templateFilePath);
            string filePath = GetFilePath(entity, domainFolder, templateFileName);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Entity entity, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForBackend(entity, domainFolder);
            string fileName = ModellNameReplacements.ReplaceEntityPlaceholders(entity, templateFileName);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }
    }
}