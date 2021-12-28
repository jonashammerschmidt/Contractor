using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityMethodsAddition : PropertyAdditionEditor
    {
        public DbEntityMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateEf" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            ef{options.EntityName}.{options.PropertyName} = db{options.EntityName}Update.{options.PropertyName};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = ef{options.EntityName}.{options.PropertyName},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("ToEf" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = db{options.EntityName}.{options.PropertyName},");

            return stringEditor.GetText();
        }
    }
}