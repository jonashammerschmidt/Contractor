using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlToRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageHtmlToRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</app-table-filter-bar>");
            stringEditor.PrevThatContains("<!-- Right -->");
            stringEditor.Next();
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        <app-table-filter-bar-dropdown [floatingRight]=\"true\" [dataSource]=\"{relationSide.NameLower}DataSource\" idExpr=\"id\" displayExpr=\"bezeichnung\"");
            stringEditor.InsertLine($"            label=\"{relationSide.Name.ToReadable()}\" [(values)]=\"{relationSide.NameLower}SelectedValues\" (valuesChange)=\"{relationSide.Entity.NamePluralLower}DataSource.triggerUpdate()\">");
            stringEditor.InsertLine("        </app-table-filter-bar-dropdown>");

            stringEditor.NextThatContains("<table mat-table");
            stringEditor.NextThatContains("<ng-container matColumnDef=\"detail\">");
            stringEditor.InsertLine(GetAppRoutingLine(relationSide));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(RelationSide relationSide)
        {
            return
             $"            <ng-container matColumnDef=\"{relationSide.NameLower}\">\n" +
             $"                <th mat-header-cell *matHeaderCellDef> {relationSide.Name.ToReadable()} </th>\n" +
              "                <td mat-cell *matCellDef=\"let element\">\n" +
             $"                    <span *ngIf=\"element.{relationSide.NameLower}\">{{{{element.{relationSide.NameLower}.{relationSide.OtherEntity.DisplayProperty.NameLower}}}}}</span>\n" +
             $"                    <span *ngIf=\"!element.{relationSide.NameLower}\">-</span>\n" +
              "                </td>\n" +
              "            </ng-container>\n";
        }
    }
}