using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageHtmlPropertyAddition : FrontendPropertyAdditionEditor
    {
        public EntityCreatePageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(FrontendPageCreatePropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}