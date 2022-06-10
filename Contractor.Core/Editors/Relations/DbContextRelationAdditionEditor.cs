namespace Contractor.Core.Tools
{
    internal abstract class DbContextRelationAdditionEditor
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbContextRelationAdditionEditor(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(RelationSide relationSide)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(relationSide, "DbContext", "DbContextName.cs");

            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);
            fileData = UpdateFileData(relationSide, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected abstract string UpdateFileData(RelationSide relationSide, string fileData);
    }
}