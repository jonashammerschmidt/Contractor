using Contractor.Core.MetaModell;
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

        public void AddPropertyToBackendFile(Property property, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackend(property, paths);
            AddPropertyToFile(property, filePath);
        }

        public void AddPropertyToDatabaseFile(Property property, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(property, paths);
            AddPropertyToFile(property, filePath);
        }

        public void AddPropertyToFrontendFile(Property property, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(property, paths);
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