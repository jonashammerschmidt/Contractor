using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbContextEntityAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbContextEntityAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IEntityAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(IEntityAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            string usingStatement = $"{options.ProjectName}.Persistence.Modules.{options.Domain}.{options.EntityNamePlural}";
            fileData = UsingStatements.Add(fileData, usingStatement);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("CustomInstantiate");

            stringEditor.InsertLine(GetDbSetLine(options));
            stringEditor.InsertNewLine();

            // ----------- OnModelCreating -----------

            stringEditor.NextThatContains("this.OnModelCreatingPartial(modelBuilder);");

            stringEditor.InsertLine(GetEmptyModelBuilderEntityLine(options));
            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }

        private string GetDbSetLine(IEntityAdditionOptions options)
        {
            return $"        public virtual DbSet<Ef{options.EntityName}> {options.EntityNamePlural}" + " { get; set; }";
        }

        private string GetEmptyModelBuilderEntityLine(IEntityAdditionOptions options)
        {
            return $"            modelBuilder.Entity<Ef{options.EntityName}>(entity =>\n" +
                 "            {\n" +
                 $"                entity.ToTable(\"{options.EntityNamePlural}\");\n" +
                 "\n" +
                 $"                entity.Property(e => e.Id).ValueGeneratedNever();\n" +
                 "            });";
        }
    }
}