using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityCreatePageHtmlToOneToOnePropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityCreatePageHtmlToOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("<div class=\"form-layout-card\">");
            stringEditor.NextThatStartsWith("            </div>");

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(relationSide));

            return stringEditor.GetText();
        }

        private string GetLine(RelationSide relationSide)
        {
            return
                $"                <div formLayoutRow [formGroupInstance]=\"formGroup\" formControlNameInstance=\"{relationSide.NameLower}Id\" >\n" +
                $"                    <mat-label>{relationSide.Name.ToReadable()}:</mat-label>\n" +
                 "                    <div class=\"form-layout-inputs\">\n" +
                $"                        <app-search-dropdown [formGroupInstance]=\"formGroup\" [formControlNameInstance]=\"'{relationSide.NameLower}Id'\"\n" +
                $"                            label=\"{relationSide.Name.ToReadable()}\" idExpr=\"id\" displayExpr=\"{relationSide.OtherEntity.DisplayProperty.NameLower}\" [dataSource]=\"{relationSide.NameLower}DataSource\"></app-search-dropdown>" +
                 "                    </div>\n" +
                 "                    <form-layout-row-status></form-layout-row-status>\n" +
                 "                    <div class=\"form-layout-side-bar\" >\n" +
                $"                        <form-layout-error formControlNameInstance=\"{relationSide.NameLower}Id\" errorType=\"required\"\n" +
                 "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                 "                        </form-layout-error>\n" +
                 "                    </div>\n" +
                 "                </div>";
        }
    }
}