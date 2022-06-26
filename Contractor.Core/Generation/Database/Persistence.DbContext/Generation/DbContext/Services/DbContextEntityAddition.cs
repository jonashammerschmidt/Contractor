using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.Linq;

namespace Contractor.Core.Generation.Database.Persistence.DbContext
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
            string filePath = this.pathService.GetAbsolutePathForDatabase(entity, $"DbContext\\{entity.Module.Options.Paths.DbContextName}Raw.cs");
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
            stringEditor.InsertLine("            {");
            stringEditor.InsertLine($"                entity.ToTable(\"{entity.NamePlural}\");");
            stringEditor.InsertLine("");

            if (entity.HasScope)
            {
                stringEditor.InsertLine($"                entity.HasKey(c => new {{ c.{entity.ScopeEntity.Name}Id, c.Id }})");
            }
            else
            {
                stringEditor.InsertLine($"                entity.HasKey(c => c.Id)");
            }

            stringEditor.InsertLine($"                    .IsClustered({(!entity.Indices.Any(index => index.IsClustered)).ToString().ToLower()});");

            foreach (var index in entity.Indices)
            {
                string indexProperties = string.Join(", ", index.ColumnNames.Select(columnName => "c." + columnName));

                stringEditor.InsertNewLine();
                stringEditor.InsertLine(
                    $"                entity.HasIndex(c => new {{ {indexProperties} }})\n" +
                    $"                    .IsUnique({index.IsUnique.ToString().ToLower()})\n" +
                    $"                    .IsClustered({index.IsClustered.ToString().ToLower()});");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine($"                entity.Property(e => e.Id)");
            if (entity.IdType == "AutoIncrement")
            {
                stringEditor.InsertLine($"                    .ValueGeneratedOnAdd();");
            }
            else
            {
                stringEditor.InsertLine($"                    .ValueGeneratedNever();");
            }

            if (entity.HasScope)
            {
                stringEditor.InsertNewLine();
                stringEditor.InsertLine($"                entity.HasOne(d => d.{entity.ScopeEntity.Name})");
                stringEditor.InsertLine("                    .WithMany()");
                stringEditor.InsertLine($"                    .HasForeignKey(d => d.{entity.ScopeEntity.Name}Id)");
                stringEditor.InsertLine("                    .IsRequired(true)");
                stringEditor.InsertLine("                    .OnDelete(DeleteBehavior.NoAction)");
                stringEditor.InsertLine($"                    .HasConstraintName(\"FK_{entity.NamePlural}_{entity.ScopeEntity.Name}Id\");");
            }

            stringEditor.InsertLine("            });");
            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }
    }
}