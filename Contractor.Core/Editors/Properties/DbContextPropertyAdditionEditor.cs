using Contractor.Core.Options;
using System.IO;

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

        public void Edit(IPropertyAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(options, $"DbContext\\{options.DbContextName}.cs");

            string fileData = this.fileSystemClient.ReadAllText(filePath);
            fileData = UpdateFileData(options, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected abstract string UpdateFileData(IPropertyAdditionOptions options, string fileData);
    }
}