using Contractor.Core.Tools;

namespace Contractor.Core.BaseClasses
{
    internal abstract class PropertyAdditionToExisitingFileGeneration
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public PropertyAdditionToExisitingFileGeneration(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToBackendFile(Property property, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForBackend(property, fileName);
            AddPropertyToFile(property, filePath);
        }

        public void AddPropertyToDatabaseFile(Property property, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(property, fileName);
            AddPropertyToFile(property, filePath);
        }

        public void AddPropertyToFrontendFile(Property property, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(property, fileName);
            AddPropertyToFile(property, filePath);
        }

        private void AddPropertyToFile(Property property, string filePath)
        {
            string fileData = fileSystemClient.ReadAllText(property, filePath);
            fileData = UpdateFileData(property, fileData);

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected abstract string UpdateFileData(Property property, string fileData);
    }
}