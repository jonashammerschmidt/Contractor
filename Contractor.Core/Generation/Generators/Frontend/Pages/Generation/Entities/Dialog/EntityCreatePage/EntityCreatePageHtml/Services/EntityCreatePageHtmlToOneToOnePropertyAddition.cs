using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class EntityCreatePageHtmlToOneToOnePropertyAddition : RelationSideAdditionToExisitingFileGeneration
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
                $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{relationSide.NameLower}Id\" >\n" +
                $"                    <mat-label>{relationSide.Name.ToReadable()}:</mat-label>\n" +
                 "                    <div class=\"form-layout-inputs\">\n" +
                $"                        <app-search-dropdown-v2 [formGroupInstance]=\"formController.formGroup\" [formControlNameInstance]=\"'{relationSide.NameLower}Id'\"\n" +
                $"                            label=\"{relationSide.Name.ToReadable()}\" displayExpr=\"{relationSide.OtherEntity.DisplayProperty.NameLower}\" [dataSource]=\"{relationSide.NameLower}DataSource\"></app-search-dropdown-v2>\n" +
                 "                    </div>\n" +
                 "                    <form-layout-row-status>\n" +
                $"                        <form-layout-error formControlNameInstance=\"{relationSide.NameLower}Id\" errorType=\"required\"\n" +
                 "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                 "                        </form-layout-error>\n" +
                 "                    </form-layout-row-status>\n" +
                 "                </div>";
        }
    }
}