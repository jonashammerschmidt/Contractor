using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlFromOneToOneRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageHtmlFromOneToOneRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
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
             $"            <ng-container matColumnDef=\"{options.PropertyNameTo.LowerFirstChar()}\">\n" +
             $"                <th mat-header-cell *matHeaderCellDef> {options.PropertyNameTo.ToReadable()} </th>\n" +
              "                <td mat-cell *matCellDef=\"let element\">\n" +
             $"                    <span *ngIf=\"element.{options.PropertyNameTo.LowerFirstChar()}\">{{{{element.{options.PropertyNameTo.LowerFirstChar()}.bezeichnung}}}}</span>\n" +
             $"                    <span *ngIf=\"!element.{options.PropertyNameTo.LowerFirstChar()}\">-</span>\n" +
              "                </td>\n" +
              "            </ng-container>\n";
        }
    }
}