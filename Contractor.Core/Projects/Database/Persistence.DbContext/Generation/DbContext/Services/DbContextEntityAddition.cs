using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
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
            string filePath = this.pathService.GetAbsolutePathForDatabase(options, $"DbContext\\{options.DbContextName}.cs");
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(IEntityAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            string usingStatement = $"{options.DbProjectName}.Persistence.DbContext.Modules.{options.Domain}.{options.EntityNamePlural}";
            fileData = UsingStatements.Add(fileData, usingStatement);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");

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
                 $"                entity.Property(e => e.Id)\n" +
                 $"                    .ValueGeneratedNever();\n" +
                 "            });";
        }
    }
}