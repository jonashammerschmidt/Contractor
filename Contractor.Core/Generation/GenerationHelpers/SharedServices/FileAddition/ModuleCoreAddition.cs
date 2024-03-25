using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.BaseClasses
{
    public class ModuleCoreAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public ModuleCoreAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddModuleToBackend(Module module, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = pathService.GetAbsolutePathForBackend(module, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(module, templateFilePath);

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        public void AddModuleToFrontend(Module module, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(module, domainFolder, templateFileName);
            string fileData = fileSystemClient.ReadAllText(module, templateFilePath);

            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}