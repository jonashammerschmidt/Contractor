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
            if (DatabaseDbContextPropertyLine.GetPropertyLine(options) == null)
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
            stringEditor.InsertLine(DatabaseDbContextPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}