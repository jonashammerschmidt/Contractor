using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendPageCreatePropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.Boolean:
                    return
                        $"        <mat-checkbox [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"            {options.PropertyName.ToReadable()}\n" +
                        $"        </mat-checkbox>\n" +
                        $"        <br>";
                case PropertyTypes.DateTime:
                    return
                        "        <mat-form-field appearance=\"outline\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\" [matDatepicker]=\"picker\">\n" +
                        "            <mat-datepicker-toggle matSuffix [for]=\"picker\"></mat-datepicker-toggle>\n" +
                        "            <mat-datepicker #picker></mat-datepicker>\n" +
                        "        </mat-form-field>";
                case PropertyTypes.Integer:
                case PropertyTypes.Double:
                    return
                        "        <mat-form-field appearance=\"outline\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput type=\"number\" placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        "        </mat-form-field>";
                case PropertyTypes.String when string.IsNullOrEmpty(options.PropertyTypeExtra):
                    return
                        "        <mat-form-field appearance=\"outline\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput maxlength=\"{options.PropertyTypeExtra}\" placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        "        </mat-form-field>";
                default:
                    return
                        "        <mat-form-field appearance=\"outline\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        "        </mat-form-field>";
            }
        }
    }
}