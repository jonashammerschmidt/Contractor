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

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateEf" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            ef{property.Entity.Name}.{property.Name} = db{property.Entity.Name}Update.{property.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = ef{property.Entity.Name}.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("ToEf" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = db{property.Entity.Name}.{property.Name},");

            return stringEditor.GetText();
        }
    }
}