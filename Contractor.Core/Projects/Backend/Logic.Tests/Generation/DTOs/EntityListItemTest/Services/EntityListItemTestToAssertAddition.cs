using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityListItemTestToAssertAddition : RelationAdditionEditor
    {
        public EntityListItemTestToAssertAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- AssertDbDefault -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            {options.EntityNameFrom}Test.AssertDefault({options.EntityNameLowerTo}ListItem.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            {options.EntityNameFrom}Test.AssertDefault2({options.EntityNameLowerTo}ListItem.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}