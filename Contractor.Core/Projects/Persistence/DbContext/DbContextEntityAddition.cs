using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contractor.Core.Projects.Persistence
{
    public class DbContextEntityAddition
    {
        public PathService pathService;

        public DbContextEntityAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(EntityOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(EntityOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Domain", options.Domain);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(EntityOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

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