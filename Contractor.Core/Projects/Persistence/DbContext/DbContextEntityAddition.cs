using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    public class DbContextEntityAddition
    {
        public UsingStatementAddition usingStatementAddition;
        public PathService pathService;

        public DbContextEntityAddition(
            UsingStatementAddition usingStatementAddition,
            PathService pathService)
        {
            this.usingStatementAddition = usingStatementAddition;
            this.pathService = pathService;
        }

        public void Add(EntityOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(EntityOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            string usingStatement = $"{options.ProjectName}.Persistence.Model.{options.Domain}.{options.EntityNamePlural}";
            fileData = this.usingStatementAddition.Add(fileData, usingStatement);

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

        private string GetDbSetLine(EntityOptions options)
        {
            return $"        public virtual DbSet<Ef{options.EntityName}> {options.EntityNamePlural}" + " { get; set; }";
        }

        private string GetEmptyModelBuilderEntityLine(EntityOptions options)
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