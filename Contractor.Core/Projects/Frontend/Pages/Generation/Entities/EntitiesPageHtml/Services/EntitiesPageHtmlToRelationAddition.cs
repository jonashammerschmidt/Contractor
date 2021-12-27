using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlToRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageHtmlToRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("</app-table-filter-bar>");
            stringEditor.PrevThatContains("<!-- Right -->");
            stringEditor.Next();
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        <app-table-filter-bar-dropdown [floatingRight]=\"true\" [dataSource]=\"{options.PropertyNameFrom.LowerFirstChar()}DataSource\" idExpr=\"id\" displayExpr=\"bezeichnung\"");
            stringEditor.InsertLine($"            label=\"{options.PropertyNameFrom.ToReadable()}\" [(values)]=\"{options.PropertyNameFrom.LowerFirstChar()}SelectedValues\" (valuesChange)=\"{options.EntityNamePluralLowerTo}DataSource.triggerUpdate()\">");
            stringEditor.InsertLine("        </app-table-filter-bar-dropdown>");

            stringEditor.NextThatContains("<table mat-table");
            stringEditor.NextThatContains("<ng-container matColumnDef=\"detail\">");
            stringEditor.InsertLine(GetAppRoutingLine(options));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(IRelationAdditionOptions options)
        {
            return
             $"            <ng-container matColumnDef=\"{options.PropertyNameFrom.LowerFirstChar()}\">\n" +
             $"                <th mat-header-cell *matHeaderCellDef> {options.PropertyNameFrom.ToReadable()} </th>\n" +
              "                <td mat-cell *matCellDef=\"let element\">\n" +
             $"                    <span *ngIf=\"element.{options.PropertyNameFrom.LowerFirstChar()}\">{{{{element.{options.PropertyNameFrom.LowerFirstChar()}.bezeichnung}}}}</span>\n" +
             $"                    <span *ngIf=\"!element.{options.PropertyNameFrom.LowerFirstChar()}\">-</span>\n" +
              "                </td>\n" +
              "            </ng-container>\n";
        }
    }
}