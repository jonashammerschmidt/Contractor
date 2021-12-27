using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlToOneToOneRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageHtmlToOneToOneRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

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