using Contractor.Core.Helpers;
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

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            string usingStatement = $"{entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{entity.Module.Name}.{entity.NamePlural}";
            fileData = UsingStatements.Add(fileData, usingStatement);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");

            stringEditor.InsertLine($"        public virtual DbSet<Ef{entity.Name}> {entity.NamePlural}" + " { get; set; }");
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("this.OnModelCreatingPartial(modelBuilder);");

            stringEditor.InsertLine($"            modelBuilder.Entity<Ef{entity.Name}>(entity =>");
            stringEditor.InsertLine( "            {");
            stringEditor.InsertLine($"                entity.ToTable(\"{entity.NamePlural}\");");
            stringEditor.InsertLine("");
            stringEditor.InsertLine($"                entity.Property(e => e.Id)");
            if (entity.IdType == "AutoIncrement")
            {
                stringEditor.InsertLine($"                    .ValueGeneratedOnAdd();");
            }
            else
            {
                stringEditor.InsertLine($"                    .ValueGeneratedNever();");
            }

            stringEditor.InsertLine("            });");
            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }
    }
}