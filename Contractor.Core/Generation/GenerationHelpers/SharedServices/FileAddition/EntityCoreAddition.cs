using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.BaseClasses
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

        public void AddEntityToBackend(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = pathService.GetAbsolutePathForBackend(entity, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(entity, templateFilePath);

            if (entity.IdType == "AutoIncrement")
            {
                fileData = fileData.Replace(
                    "public Guid Id { get; set; }",
                    "public int Id { get; set; }");
                    
                fileData = fileData.Replace(
                    $"Guid id",
                    $"int id");
            }

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        public void AddEntityToBackendGenerated(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = pathService.GetAbsolutePathForBackendGenerated(entity, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(entity, templateFilePath);

            if (entity.IdType == "AutoIncrement")
            {
                fileData = fileData.Replace(
                    "public Guid Id { get; set; }",
                    "public int Id { get; set; }");
                    
                fileData = fileData.Replace(
                    $"(Guid {entity.NameLower}Id)",
                    $"(int {entity.NameLower}Id)");
                    
                fileData = fileData.Replace(
                    $"Guid id",
                    $"int id");
            }

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        public void AddEntityToDatabase(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(entity, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(entity, templateFilePath);

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        public void AddEntityToFrontend(Entity entity, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(entity, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(entity, templateFilePath);

            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}