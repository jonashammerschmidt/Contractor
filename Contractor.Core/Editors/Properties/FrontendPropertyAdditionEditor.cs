using Contractor.Core.Helpers;
using Contractor.Core.Options;
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
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontend(options, domainFolder);
            string filePath = Path.Combine(absolutePathForDTOs, templateFileName);

            filePath = filePath.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            filePath = filePath.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            filePath = filePath.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));

            return filePath;
        }

        protected abstract string UpdateFileData(IPropertyAdditionOptions options, string fileData);
    }
}