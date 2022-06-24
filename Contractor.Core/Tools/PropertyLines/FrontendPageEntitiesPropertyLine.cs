using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendPageEntitiesPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            switch (property.Type)
            {
                case PropertyTypes.Boolean:
                    return
                        $"            <ng-container matColumnDef=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                <th mat-header-cell *matHeaderCellDef> {property.Name.ToReadable()} </th>\n" +
                        $"                <td mat-cell *matCellDef=\"let element\">\n" +
                        $"                    <mat-icon color=\"accent\" *ngIf=\"element.{property.Name.LowerFirstChar()}\">\n" +
                        $"                        check_box\n" +
                        $"                    </mat-icon>\n" +
                        $"                    <mat-icon style=\"color: gray\" *ngIf=\"!element.{property.Name.LowerFirstChar()}\">\n" +
                        $"                        check_box_outline_blank\n" +
                        $"                    </mat-icon>\n" +
                        $"                </td>\n" +
                        $"            </ng-container>\n";
                case PropertyTypes.DateTime:
                    return
                        $"            <ng-container matColumnDef=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                <th mat-header-cell *matHeaderCellDef> {property.Name.ToReadable()} </th>\n" +
                        $"                <td mat-cell *matCellDef=\"let element\">\n" +
                        $"                    <span *ngIf=\"!element.{property.Name.LowerFirstChar()}\">-</span>\n" +
                        $"                    <span *ngIf=\"element.{property.Name.LowerFirstChar()}\">{{{{element.{property.Name.LowerFirstChar()} | date:'dd. MMM. yyyy, HH:mm'}}}}</span>\n" +
                        $"                </td>\n" +
                        "            </ng-container>\n";
                default:
                    return
                         $"            <ng-container matColumnDef=\"{property.Name.LowerFirstChar()}\">\n" +
                         $"                <th mat-header-cell *matHeaderCellDef> {property.Name.ToReadable()} </th>\n" +
                         $"                <td mat-cell *matCellDef=\"let element\">\n" +
                         $"                    <span *ngIf=\"!element.{property.Name.LowerFirstChar()}\">-</span>\n" +
                         $"                    <span *ngIf=\"element.{property.Name.LowerFirstChar()}\">{{{{element.{property.Name.LowerFirstChar()}}}}}</span>\n" +
                         $"                </td>\n" +
                          "            </ng-container>\n";
            }
        }
    }
}