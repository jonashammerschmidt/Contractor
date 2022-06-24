using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class DtoAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DtoAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddDto(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            AddDto(entity, domainFolder, templateFilePath, templateFileName, false);
        }

        public void AddDto(Entity entity, string domainFolder, string templateFilePath, string templateFileName, bool forDatabase)
        {
            string filePath = (forDatabase) ?
                this.pathService.GetAbsolutePathForDatabase(entity, domainFolder, templateFileName) :
                this.pathService.GetAbsolutePathForBackend(entity, domainFolder, templateFileName);
            string fileData = this.fileSystemClient.ReadAllText(entity, templateFilePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}