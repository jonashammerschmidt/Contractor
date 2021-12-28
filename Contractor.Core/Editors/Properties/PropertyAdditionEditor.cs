using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal abstract class PropertyAdditionEditor
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public PropertyAdditionEditor(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(IPropertyAdditionOptions options, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);

            string fileData = this.fileSystemClient.ReadAllText(filePath);
            foreach (string namespaceToAdd in namespacesToAdd)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            fileData = UpdateFileData(options, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForBackend(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        protected abstract string UpdateFileData(IPropertyAdditionOptions options, string fileData);
    }
}