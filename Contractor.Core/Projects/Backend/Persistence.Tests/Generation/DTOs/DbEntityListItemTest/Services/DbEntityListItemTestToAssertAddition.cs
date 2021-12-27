using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityListItemTestToAssertAddition : RelationAdditionEditor
    {
        public DbEntityListItemTestToAssertAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- AssertDbDefault -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDbDefault(db{options.EntityNameTo}ListItem.{options.PropertyNameFrom});");

            stringEditor.NextThatContains("AssertDbDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDbDefault2(db{options.EntityNameTo}ListItem.{options.PropertyNameFrom});");

            return stringEditor.GetText();
        }
    }
}