using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbContextPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbContextPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options)
        {
            if (DatabaseDbContextPropertyLine.GetPropertyLine(options) == null)
            {
                return;
            }

            string filePath = this.pathService.GetAbsolutePathForBackend(options, "Persistence\\PersistenceDbContext.cs");
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityName}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(DatabaseDbContextPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}