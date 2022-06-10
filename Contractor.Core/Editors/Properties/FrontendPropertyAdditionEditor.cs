using System.IO;

namespace Contractor.Core.Tools
{
    internal abstract class FrontendPropertyAdditionEditor
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public FrontendPropertyAdditionEditor(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(Property property, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = GetFilePath(property, domainFolder, templateFileName);

            string fileData = this.fileSystemClient.ReadAllText(property, filePath);
            foreach (string namespaceToAdd in namespacesToAdd)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            fileData = UpdateFileData(property, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Property property, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontend(property, domainFolder);
            string filePath = Path.Combine(absolutePathForDTOs, templateFileName);
            return filePath;
        }

        protected abstract string UpdateFileData(Property property, string fileData);
    }
}