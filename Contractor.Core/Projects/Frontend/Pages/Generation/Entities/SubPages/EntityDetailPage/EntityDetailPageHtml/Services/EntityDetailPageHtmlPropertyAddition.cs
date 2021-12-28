using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlPropertyAddition : FrontendPropertyAdditionEditor
    {
        public EntityDetailPageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            if (fileData.Contains("</mat-tab-group>"))
            {
                stringEditor.NextThatContains("<mat-tab label=\"Stammdaten\">");
                stringEditor.NextThatContains("</mat-tab>");
            }
            else
            {
                stringEditor.NextThatStartsWith($"<div class=\"{options.EntityName.ToKebab()}-detail-page\"");
                stringEditor.NextThatStartsWith($"</div>");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(FrontendPageDetailPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}