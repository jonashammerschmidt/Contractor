using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityListItemTestMethodsAddition : PropertyAdditionEditor
    {
        public DbEntityListItemTestMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}DbDefault, db{options.EntityName}ListItem.{options.PropertyName});");
            
            stringEditor.NextThatContains("AssertDbDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}DbDefault2, db{options.EntityName}ListItem.{options.PropertyName});");

            return stringEditor.GetText();
        }
    }
}