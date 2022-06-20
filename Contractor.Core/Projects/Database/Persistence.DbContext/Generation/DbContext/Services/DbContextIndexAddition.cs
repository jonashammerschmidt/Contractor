using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System.Linq;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class DbContextIndexAddition
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public DbContextIndexAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Entity entity)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(entity, $"DbContext\\{entity.Module.Options.Paths.DbContextName}.cs");
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);
            fileData = UpdateFileData(entity, fileData);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected string UpdateFileData(Entity entity, string fileData)
        {

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{entity.Name}>");
            stringEditor.NextThatContains("});");

            if (entity.Indices.Any(index => index.IsClustered))
            {
                stringEditor.InsertNewLine();
                stringEditor.InsertLine(
                    $"                entity.HasKey(c => c.Id)\n" +
                    $"                    .IsClustered(false);");
            }

            foreach (var index in entity.Indices)
            {
                string indexProperties = string.Join(", ", index.ColumnNames.Select(columnName => "c." + columnName));

                stringEditor.InsertNewLine();
                stringEditor.InsertLine(
                    $"                entity.HasIndex(c => new {{ {indexProperties} }})\n" +
                    $"                    .IsUnique({index.IsUnique.ToString().ToLower()})\n" +
                    $"                    .IsClustered({index.IsClustered.ToString().ToLower()});");
            }

            return stringEditor.GetText();
        }
    }
}