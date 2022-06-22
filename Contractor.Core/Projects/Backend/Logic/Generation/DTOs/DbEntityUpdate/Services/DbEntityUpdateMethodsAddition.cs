using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class DbEntityUpdateMethodsAddition : PropertyAdditionEditor
    {
        public DbEntityUpdateMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("From" + property.Entity.Name + "Update(");
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {property.Name} = {property.Entity.NameLower}Update.{property.Name},");
            
            return stringEditor.GetText();
        }
    }
}