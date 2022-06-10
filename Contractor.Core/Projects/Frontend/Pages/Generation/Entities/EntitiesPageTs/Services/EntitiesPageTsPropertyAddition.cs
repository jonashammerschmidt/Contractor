using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsPropertyAddition : FrontendPropertyAdditionEditor
    {
        public EntitiesPageTsPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");

            stringEditor.InsertLine($"    '{property.Name.LowerFirstChar()}',");

            return stringEditor.GetText();
        }
    }
}