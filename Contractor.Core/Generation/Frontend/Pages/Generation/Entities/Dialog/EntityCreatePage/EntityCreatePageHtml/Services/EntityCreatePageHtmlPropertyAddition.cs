using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityCreatePageHtmlPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityCreatePageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");
            stringEditor.Prev();

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(FrontendPageCreateUpdatePropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}