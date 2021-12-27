using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryFromOneToOneIncludeAddition : RelationAdditionEditor
    {
        public EntitiesCrudRepositoryFromOneToOneIncludeAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "Microsoft.EntityFrameworkCore");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"Get{options.EntityNameFrom}Detail(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralFrom}");
            stringEditor.Next(line => !line.Contains("Include("));
            stringEditor.InsertLine($"                .Include(ef{options.EntityNameFrom} => ef{options.EntityNameFrom}.{options.PropertyNameTo})");
            stringEditor.MoveToStart();

            string includeLine = $"                .Include(ef{options.EntityNameFrom} => ef{options.EntityNameFrom}.{options.PropertyNameTo})";
            stringEditor.NextThatContains($"GetPaged{options.EntityNamePluralFrom}(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralFrom}");
            stringEditor.Next(line => !line.Contains("Include("));
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