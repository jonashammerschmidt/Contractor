using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlPropertyAddition : FrontendPropertyAdditionEditor
    {
        public EntityUpdatePageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");

            stringEditor.InsertLine(FrontendPageUpdatePropertyLine.GetPropertyLine(property));

            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }
    }
}