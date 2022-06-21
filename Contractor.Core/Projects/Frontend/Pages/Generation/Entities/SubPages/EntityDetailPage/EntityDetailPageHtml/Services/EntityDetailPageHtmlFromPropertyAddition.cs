using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlFromPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntityDetailPageHtmlFromPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            if (!fileData.Contains("</mat-tab-group>"))
            {
                AddTabs(stringEditor);
            }

            AddTab(relationSide, stringEditor);

            return stringEditor.GetText();
        }

        private void AddTabs(StringEditor stringEditor)
        {
            stringEditor.NextThatStartsWith("    <div class=\"toolbar\">");
            stringEditor.NextThatStartsWith("    </div>");
            stringEditor.Next();

            stringEditor.InsertLine("    <mat-tab-group mat-align-tabs=\"start\">");
            stringEditor.InsertLine("        <mat-tab label=\"Stammdaten\">");

            stringEditor.NextThatStartsWith("</div>");

            stringEditor.InsertLine("        </mat-tab>");
            stringEditor.InsertLine("    </mat-tab-group>");

            stringEditor.AddPrefixBetweenLinesThatContain("        ", "<mat-tab label=\"Stammdaten\">", " </mat-tab>");
        }

        private void AddTab(RelationSide relationSide, StringEditor stringEditor)
        {
            stringEditor.MoveToStart();
            stringEditor.NextThatContains("    </mat-tab-group>");

            stringEditor.InsertLine($"        <mat-tab label=\"{relationSide.Name.ToReadable()}\">");
            stringEditor.InsertLine($"            <h2>{relationSide.Name.ToReadable()}</h2>");
            stringEditor.InsertLine($"            <div class=\"table-container\">");
            stringEditor.InsertLine($"                <table mat-table [dataSource]=\"{relationSide.NameLower}TableDataSource\">");
            stringEditor.InsertLine($"");
            stringEditor.InsertLine($"                    <ng-container matColumnDef=\"{relationSide.OtherEntity.DisplayProperty.NameLower}\">");
            stringEditor.InsertLine($"                        <th mat-header-cell *matHeaderCellDef> {relationSide.OtherEntity.DisplayProperty.Name.ToReadable()} </ th > ");
            stringEditor.InsertLine($"                        <td mat-cell *matCellDef=\"let element\">");
            stringEditor.InsertLine($"                            {{{{element.{relationSide.OtherEntity.DisplayProperty.NameLower}}}}}");
            stringEditor.InsertLine($"                        </td>");
            stringEditor.InsertLine($"                    </ng-container>");
            stringEditor.InsertLine($"");
            stringEditor.InsertLine($"                    <ng-container matColumnDef=\"detail\">");
            stringEditor.InsertLine($"                        <th mat-header-cell *matHeaderCellDef></th>");
            stringEditor.InsertLine($"                        <td mat-cell *matCellDef=\"let element\" width=\"10%\">");
            stringEditor.InsertLine($"                            <button mat-button role=\"link\">Detail</button>");
            stringEditor.InsertLine($"                        </td>");
            stringEditor.InsertLine($"                    </ng-container>");
            stringEditor.InsertLine($"");
            stringEditor.InsertLine($"                    <tr mat-header-row *matHeaderRowDef=\"{relationSide.NameLower}GridColumns; sticky: true\"></tr>");
            stringEditor.InsertLine($"                    <tr mat-row *matRowDef=\"let row; columns: {relationSide.NameLower}GridColumns;\"");
            stringEditor.InsertLine($"                        [routerLink]=\"['/{relationSide.Entity.Module.NameKebab}/{relationSide.OtherEntity.NamePluralKebab}/detail', row.id]\"></tr>");
            stringEditor.InsertLine($"                </table>");
            stringEditor.InsertLine($"            </div>");
            stringEditor.InsertLine($"        </mat-tab>");
        }
    }
}