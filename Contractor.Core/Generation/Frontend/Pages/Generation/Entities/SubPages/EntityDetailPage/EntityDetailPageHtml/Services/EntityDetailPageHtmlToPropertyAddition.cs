using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityDetailPageHtmlToPropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityDetailPageHtmlToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");
            stringEditor.Prev();

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(relationSide));

            return stringEditor.GetText();
        }

        private string GetLine(RelationSide relationSide)
        {
            return
                $"                    <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{relationSide.NameLower}Id\" >\n" +
                $"                        <mat-label>{relationSide.Name.ToReadable()}:</mat-label>\n" +
                 "                        <div class=\"form-layout-inputs\">\n" +
                $"                            <app-search-dropdown [formGroupInstance]=\"formController.formGroup\" [formControlNameInstance]=\"'{relationSide.NameLower}Id'\"\n" +
                $"                            label=\"{relationSide.Name.ToReadable()}\" idExpr=\"id\" displayExpr=\"{relationSide.OtherEntity.DisplayProperty.NameLower}\" [dataSource]=\"{relationSide.NameLower}DataSource\"></app-search-dropdown>\n" +
                 "                        </div>\n" +
                 "                        <form-layout-row-status></form-layout-row-status>\n" +
                 "                        <div class=\"form-layout-side-bar\" >\n" +
                $"                            <form-layout-error formControlNameInstance=\"{relationSide.NameLower}Id\" errorType=\"required\"\n" +
                 "                                title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                 "                            </form-layout-error>\n" +
                 "                        </div>\n" +
                 "                    </div>";
        }
    }
}