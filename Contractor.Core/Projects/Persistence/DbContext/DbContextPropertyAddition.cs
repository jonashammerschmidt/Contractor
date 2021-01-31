using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    public class DbContextPropertyAddition
    {
        public PathService pathService;

        public DbContextPropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(PropertyOptions options, string domainFolder, string templateFileName)
        {
            if (this.GetEntityDbModel(options) == null)
            {
                return;
            }

            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(PropertyOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Domain", options.Domain);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(PropertyOptions options, string filePath)
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

        private string GetEntityDbModel(PropertyOptions options)
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