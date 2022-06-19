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

        public void Edit(Property property, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForBackend(property, domainFolder, templateFileName);

            string fileData = this.fileSystemClient.ReadAllText(property, filePath);
            foreach (string namespaceToAdd in namespacesToAdd)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            fileData = UpdateFileData(property, fileData);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected abstract string UpdateFileData(Property property, string fileData);
    }
}