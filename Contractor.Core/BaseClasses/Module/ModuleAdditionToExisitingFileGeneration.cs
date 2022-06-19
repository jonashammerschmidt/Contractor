using Contractor.Core.Tools;

namespace Contractor.Core.BaseClasses
{
    internal abstract class ModuleAdditionToExisitingFileGeneration
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public ModuleAdditionToExisitingFileGeneration(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddModuleToBackendFile(Module module, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForBackend(module, fileName);
            AddModuleToFile(module, filePath);
        }

        public void AddModuleToDatabaseFile(Module module, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(module, fileName);
            AddModuleToFile(module, filePath);
        }

        public void AddModuleToFrontendFile(Module module, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(module, fileName);
            AddModuleToFile(module, filePath);
        }

        private void AddModuleToFile(Module module, string filePath)
        {
            string fileData = fileSystemClient.ReadAllText(module, filePath);
            fileData = UpdateFileData(module, fileData);

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected abstract string UpdateFileData(Module module, string fileData);
    }
}