using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbContextRelationToAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbContextRelationToAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityNameTo}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"                entity.HasOne(d => d.{options.PropertyNameFrom})\n" +
                    $"                    .WithMany(p => p.{options.PropertyNameTo})\n" +
                    $"                    .HasForeignKey(d => d.{options.PropertyNameFrom}Id)\n" +
                    $"                    .HasConstraintName(\"FK_{options.EntityNamePluralTo}_{options.PropertyNameFrom}Id\");");

            return stringEditor.GetText();
        }
    }
}