using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityDetailTestToAssertAddition : RelationAdditionEditor
    {
        public DbEntityDetailTestToAssertAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Creators -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityNameTo}Detail Default()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameFrom} = Db{options.EntityNameFrom}Test.Default(),");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityNameTo}Detail Default2()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameFrom} = Db{options.EntityNameFrom}Test.Default2(),");
            fileData = stringEditor.GetText();

            // ----------- Asserts -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDefault(db{options.EntityNameTo}Detail.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDefault2(db{options.EntityNameTo}Detail.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}