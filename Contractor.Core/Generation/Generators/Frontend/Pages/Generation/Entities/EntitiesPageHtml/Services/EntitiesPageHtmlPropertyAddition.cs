using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntitiesPageHtmlPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntitiesPageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("<table mat-table");
            stringEditor.NextThatContains("<ng-container matColumnDef=\"detail\">");

            stringEditor.InsertLine(FrontendPageEntitiesPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}