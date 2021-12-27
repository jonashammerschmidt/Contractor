using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryFromIncludeAddition : RelationAdditionEditor
    {
        public EntitiesCrudRepositoryFromIncludeAddition(IFileSystemClient fileSystemClient, PathService pathService)
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

            return stringEditor.GetText();
        }
    }
}