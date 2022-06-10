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

        public void Add(Entity entity)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(entity, $"DbContext\\{entity.Module.Options.Paths.DbContextName}.cs");
            string fileData = UpdateFileData(entity, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            string usingStatement = $"{entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{entity.Module.Name}.{entity.NamePlural}";
            fileData = UsingStatements.Add(fileData, usingStatement);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");

            stringEditor.InsertLine(GetDbSetLine(entity));
            stringEditor.InsertNewLine();

            // ----------- OnModelCreating -----------

            stringEditor.NextThatContains("this.OnModelCreatingPartial(modelBuilder);");

            stringEditor.InsertLine(GetEmptyModelBuilderEntityLine(entity));
            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }

        private string GetDbSetLine(Entity entity)
        {
            return $"        public virtual DbSet<Ef{entity.Name}> {entity.NamePlural}" + " { get; set; }";
        }

        private string GetEmptyModelBuilderEntityLine(Entity entity)
        {
            return $"            modelBuilder.Entity<Ef{entity.Name}>(entity =>\n" +
                 "            {\n" +
                 $"                entity.ToTable(\"{entity.NamePlural}\");\n" +
                 "\n" +
                 $"                entity.Property(e => e.Id)\n" +
                 $"                    .ValueGeneratedNever();\n" +
                 "            });";
        }
    }
}