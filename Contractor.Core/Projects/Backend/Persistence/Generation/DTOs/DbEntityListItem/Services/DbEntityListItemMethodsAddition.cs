using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityListItemMethodsAddition : PropertyAdditionEditor
    {
        public DbEntityListItemMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + property.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = ef{property.Entity.Name}.{property.Name},");

            return stringEditor.GetText();
        }
    }
}