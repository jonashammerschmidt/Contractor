using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityListItemTestFromOneToOneAssertAddition : RelationAdditionEditor
    {
        public DbEntityListItemTestFromOneToOneAssertAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // ----------- AssertDbDefault -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameTo}Test.AssertDbDefault(db{options.EntityNameFrom}ListItem.{options.PropertyNameTo});");

            stringEditor.NextThatContains("AssertDbDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameTo}Test.AssertDbDefault2(db{options.EntityNameFrom}ListItem.{options.PropertyNameTo});");

            return stringEditor.GetText();
        }
    }
}