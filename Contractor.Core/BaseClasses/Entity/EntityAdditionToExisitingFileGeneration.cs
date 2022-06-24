using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.BaseClasses
{
    internal abstract class EntityAdditionToExisitingFileGeneration
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EntityAdditionToExisitingFileGeneration(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddEntityToBackendFile(Entity entity, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForBackend(entity, fileName);
            AddEntityToFile(entity, filePath);
        }

        public void AddEntityToDatabaseFile(Entity entity, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(entity, fileName);
            AddEntityToFile(entity, filePath);
        }

        public void AddEntityToFrontendFile(Entity entity, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(entity, fileName);
            AddEntityToFile(entity, filePath);
        }

        protected string ReadFile(Entity entity, params string[] filePath)
        {
            return fileSystemClient.ReadAllText(entity, filePath);
        }

        protected abstract string UpdateFileData(Entity entity, string fileData);

        private void AddEntityToFile(Entity entity, string filePath)
        {
            string fileData = ReadFile(entity, filePath);
            fileData = UpdateFileData(entity, fileData);

            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}