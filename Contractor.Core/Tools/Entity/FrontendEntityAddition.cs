using Contractor.Core.MetaModell;

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
            string filePath = pathService.GetAbsolutePathForFrontend(entity, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(entity, templateFilePath);

            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}