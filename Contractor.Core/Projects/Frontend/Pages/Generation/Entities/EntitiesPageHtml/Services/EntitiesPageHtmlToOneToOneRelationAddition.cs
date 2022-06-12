using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlToOneToOneRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageHtmlToOneToOneRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

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