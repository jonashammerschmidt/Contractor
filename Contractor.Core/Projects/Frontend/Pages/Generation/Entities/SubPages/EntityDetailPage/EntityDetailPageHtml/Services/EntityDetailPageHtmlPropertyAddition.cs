using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDetailPageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            if (fileData.Contains("</mat-tab-group>"))
            {
                stringEditor.NextThatContains("<mat-tab label=\"Stammdaten\">");
                stringEditor.NextThatContains("</mat-tab>");
            }
            else
            {
                stringEditor.NextThatStartsWith($"<div class=\"{property.Entity.Name.ToKebab()}-detail-page\"");
                stringEditor.NextThatStartsWith($"</div>");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(FrontendPageDetailPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}