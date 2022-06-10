namespace Contractor.Core.Tools
{
    internal abstract class FrontendRelationAdditionEditor
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public FrontendRelationAdditionEditor(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(RelationSide relationSide, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForFrontend(relationSide, domainFolder, templateFileName);

            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);
            fileData = UpdateFileData(relationSide, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected abstract string UpdateFileData(RelationSide relationSide, string fileData);
    }
}