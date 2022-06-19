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
            string filePath = this.pathService.GetAbsolutePathForBackend(entity, domainFolder, templateFileName);
            string fileData = this.fileSystemClient.ReadAllText(entity, templateFilePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}