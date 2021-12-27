using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityDetailTestFromAssertAddition : RelationAdditionEditor
    {
        public DbEntityDetailTestFromAssertAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "System.Linq");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            Db{options.EntityNameTo}Test.AssertDbDefault(db{options.EntityNameFrom}Detail.{options.PropertyNameTo}.ToArray()[0]);");

            return stringEditor.GetText();
        }
    }
}