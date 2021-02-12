using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    public class DbContextRelationToAddition
    {
        public PathService pathService;

        public DbContextRelationToAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityNameTo}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"                entity.HasOne(d => d.{options.EntityNameFrom})\n" +
                    $"                    .WithMany(p => p.{options.EntityNamePluralTo})\n" +
                    $"                    .HasForeignKey(d => d.{options.EntityNameFrom}Id)\n" +
                    $"                    .HasConstraintName(\"FK_{options.EntityNamePluralTo}_{options.EntityNameFrom}Id\");");

            return stringEditor.GetText();
        }
    }
}