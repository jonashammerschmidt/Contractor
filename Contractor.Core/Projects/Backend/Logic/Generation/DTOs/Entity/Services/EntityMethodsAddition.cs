using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityMethodsAddition : PropertyAdditionEditor
    {
        public EntityMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateDb" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            db{options.EntityName}.{options.PropertyName} = {options.EntityNameLower}Update.{options.PropertyName};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromDb" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = db{options.EntityName}.{options.PropertyName},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("CreateDb" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityNameLower}Create.{options.PropertyName},");

            return stringEditor.GetText();
        }
    }
}