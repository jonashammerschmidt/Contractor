using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class DbEntityUpdateMethodsAddition : PropertyAdditionEditor
    {
        public DbEntityUpdateMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("From" + options.EntityName + "Update(");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityNameLower}Update.{options.PropertyName},");
            
            return stringEditor.GetText();
        }
    }
}