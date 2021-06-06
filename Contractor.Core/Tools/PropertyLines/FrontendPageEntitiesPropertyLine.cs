using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendPageEntitiesPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.Boolean:
                    return
                        $"            <ng-container matColumnDef=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"                <th mat-header-cell *matHeaderCellDef> {options.PropertyName.ToReadable()} </th>\n" +
                        $"                <td mat-cell *matCellDef=\"let element\">\n" +
                        $"                    <mat-icon color=\"accent\" *ngIf=\"element.{options.PropertyName.LowerFirstChar()}\" >\n" +
                        $"                        check_box\n" +
                        $"                    </mat-icon>\n" +
                        $"                    <mat-icon style=\"color: gray\" *ngIf=\"!element.{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"                        check_box_outline_blank\n" +
                        $"                    </mat-icon>\n" +
                        $"                </td>\n" +
                        $"            </ng-container>\n";
                case PropertyTypes.DateTime:
                    return
                        $"            <ng-container matColumnDef=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"                <th mat-header-cell *matHeaderCellDef> {options.PropertyName.ToReadable()} </th>\n" +
                        $"                <td mat-cell *matCellDef=\"let element\"> {{{{element.{options.PropertyName.LowerFirstChar()} | date:'dd. MMM. yyyy, HH:mm'}}}} </td>\n" +
                        "            </ng-container>\n";
                default:
                    return
                         $"            <ng-container matColumnDef=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                         $"                <th mat-header-cell *matHeaderCellDef> {options.PropertyName.ToReadable()} </th>\n" +
                         $"                <td mat-cell *matCellDef=\"let element\"> {{{{element.{options.PropertyName.LowerFirstChar()}}}}} </td>\n" +
                          "            </ng-container>\n";
            }
        }
    }
}