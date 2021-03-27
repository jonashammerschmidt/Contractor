using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbContextPropertyAddition
    {
        public PathService pathService;

        public DbContextPropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options)
        {
            if (this.GetEntityDbModel(options) == null)
            {
                return;
            }

            string filePath = this.pathService.GetAbsolutePathForDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityName}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(GetEntityDbModel(options));

            return stringEditor.GetText();
        }

        private string GetEntityDbModel(IPropertyAdditionOptions options)
        {
            if (options.PropertyType.Contains("string"))
            {
                return $"                entity.Property(e => e.{options.PropertyName})\n" +
                        "                    .IsRequired()\n" +
                       $"                    .HasMaxLength({options.PropertyTypeExtra});";
            }
            else if (options.PropertyType.Contains("DateTime"))
            {
                return $"                entity.Property(e => e.{options.PropertyName}).HasColumnType(\"datetime\");";
            }

            return null;
        }
    }
}