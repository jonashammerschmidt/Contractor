namespace Contractor.Core.Tools
{
    internal abstract class DbContextPropertyAdditionEditor
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public DbContextPropertyAdditionEditor(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(Property property)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(property, $"DbContext\\{property.Entity.Module.Options.Paths.DbContextName}.cs");

            string fileData = this.fileSystemClient.ReadAllText(property, filePath);
            fileData = UpdateFileData(property, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected abstract string UpdateFileData(Property property, string fileData);
    }
}