using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.Persistence
{
    public class EntitiesReadRepositoryDefaultToIncludeAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntitiesReadRepositoryDefaultToIncludeAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "Microsoft.EntityFrameworkCore");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"Get{relationSide.Entity.Name}Detail(");
            stringEditor.NextThatContains($"this.dbContext.{relationSide.Entity.NamePlural}");
            stringEditor.NextUntil(line => !line.Contains("Include("));
            stringEditor.InsertLine($"                .Include(ef{relationSide.Entity.Name}Dto => ef{relationSide.Entity.Name}Dto.{relationSide.Name})");
            stringEditor.MoveToStart();

            string includeLine = $"                .Include(ef{relationSide.Entity.Name}Dto => ef{relationSide.Entity.Name}Dto.{relationSide.Name})";
            stringEditor.NextThatContains($"GetPaged{relationSide.Entity.NamePlural}(");
            stringEditor.NextThatContains($"this.dbContext.{relationSide.Entity.NamePlural}");
            stringEditor.NextUntil(line => !line.Contains("Include("));
            stringEditor.Prev();
            if (stringEditor.GetLine().Contains(";"))
            {
                stringEditor.SetLine(stringEditor.GetLine().Replace(";", ""));
            }
            stringEditor.Next();

            if (!stringEditor.GetLine().Trim().StartsWith("."))
            {
                includeLine += ";";
            }
            stringEditor.InsertLine(includeLine);

            return stringEditor.GetText();
        }
    }
}