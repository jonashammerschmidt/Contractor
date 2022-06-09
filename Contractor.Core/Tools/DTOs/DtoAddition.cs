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
            string filePath = GetFilePath(entity, domainFolder, templateFileName, forDatabase);
            string fileData = this.fileSystemClient.ReadAllText(entity, templateFilePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Entity entity, string domainFolder, string templateFileName, bool forDatabase)
        {
            string absolutePathForDTOs = (forDatabase) ?
                this.pathService.GetAbsolutePathForDatabase(entity, domainFolder) :
                this.pathService.GetAbsolutePathForBackend(entity, domainFolder);
            string fileName = ModellNameReplacements.ReplaceEntityPlaceholders(entity, templateFileName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }
    }
}