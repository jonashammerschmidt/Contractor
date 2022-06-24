using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Database.Persistence.DbContext
{
    internal class DbContextPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public DbContextPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            if (DatabaseDbContextPropertyLine.GetPropertyLine(property) == null)
            {
                return fileData;
            }

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{property.Entity.Name}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(DatabaseDbContextPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}