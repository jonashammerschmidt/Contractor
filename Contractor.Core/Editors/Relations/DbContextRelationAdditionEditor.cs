using Contractor.Core.Options;
using Contractor.Core.Tools;

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

        public void Edit(IRelationAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(options, $"DbContext\\{options.DbContextName}.cs");

            string fileData = this.fileSystemClient.ReadAllText(filePath);
            fileData = UpdateFileData(options, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected abstract string UpdateFileData(IRelationAdditionOptions options, string fileData);
    }
}