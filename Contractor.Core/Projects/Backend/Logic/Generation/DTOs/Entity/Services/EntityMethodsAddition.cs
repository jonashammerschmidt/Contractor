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

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateDb" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            db{property.Entity.Name}.{property.Name} = {property.Entity.NameLower}Update.{property.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromDb" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = db{property.Entity.Name}.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("CreateDb" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = {property.Entity.NameLower}Create.{property.Name},");

            return stringEditor.GetText();
        }
    }
}