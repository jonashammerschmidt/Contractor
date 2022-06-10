namespace Contractor.Core.Tools
{
    internal abstract class RelationAdditionEditor
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public RelationAdditionEditor(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            this.Edit(relationSide, domainFolder, templateFileName, namespacesToAdd, false);
        }

        public void EditForDatabase(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            this.Edit(relationSide, domainFolder, templateFileName, namespacesToAdd, true);
        }

        private void Edit(RelationSide relationSide, string domainFolder, string templateFileName, string[] namespacesToAdd, bool forDatabase)
        {
            string filePath =
                (forDatabase) ?
                    this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName) :
                    this.pathService.GetAbsolutePathForBackend(relationSide, domainFolder, templateFileName);

            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);
            foreach (string namespaceToAdd in namespacesToAdd)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            fileData = UpdateFileData(relationSide, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected abstract string UpdateFileData(RelationSide relationSide, string fileData);
    }
}