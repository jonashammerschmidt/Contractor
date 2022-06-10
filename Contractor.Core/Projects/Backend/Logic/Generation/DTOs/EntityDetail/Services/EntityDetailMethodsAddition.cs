using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailMethodsAddition : PropertyAdditionEditor
    {
        public EntityDetailMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromDb" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = db{property.Entity.Name}Detail.{property.Name},");

            return stringEditor.GetText();
        }
    }
}