using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Persistence
{
    internal class EntitiesCrudRepositoryToOneToOneIncludeAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntitiesCrudRepositoryToOneToOneIncludeAddition(IFileSystemClient fileSystemClient, PathService pathService)
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
            stringEditor.InsertLine($"                .Include(ef{relationSide.Entity.Name} => ef{relationSide.Entity.Name}.{relationSide.Name})");
            stringEditor.MoveToStart();

            stringEditor.NextThatContains($"GetPaged{relationSide.Entity.NamePlural}(");
            stringEditor.NextThatContains($"this.dbContext.{relationSide.Entity.NamePlural}");
            stringEditor.NextUntil(line => !line.Contains("Include("));
            stringEditor.Prev();
            if (stringEditor.GetLine().Contains(";"))
            {
                stringEditor.SetLine(stringEditor.GetLine().Replace(";", ""));
            }
            stringEditor.Next();

            string includeLine = $"                .Include(ef{relationSide.Entity.Name} => ef{relationSide.Entity.Name}.{relationSide.Name})";
            if (!stringEditor.GetLine().Trim().StartsWith("."))
            {
                includeLine += ";";
            }
            stringEditor.InsertLine(includeLine);

            return stringEditor.GetText();
        }

    }
}