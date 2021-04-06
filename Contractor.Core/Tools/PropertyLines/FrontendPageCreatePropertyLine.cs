using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendPageCreatePropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            string requiredLine = (options.IsOptional ? "" : "required=\"true\"");
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
                        "        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput {requiredLine} placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\" [matDatepicker]=\"picker\">\n" +
                        "            <mat-datepicker-toggle matSuffix [for]=\"picker\"></mat-datepicker-toggle>\n" +
                        "            <mat-datepicker #picker></mat-datepicker>\n" +
                        "        </mat-form-field>";

                case PropertyTypes.Integer:
                case PropertyTypes.Double:
                    return
                        "        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput type=\"number\" {requiredLine} placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        "        </mat-form-field>";

                case PropertyTypes.String:
                    return
                        "        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput {requiredLine} maxlength=\"{options.PropertyTypeExtra}\" placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"            <mat-hint [align]=\"'end'\">{{{{{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}.length}}}} / {options.PropertyTypeExtra}</mat-hint>\n" +
                        "        </mat-form-field>";

                case PropertyTypes.Guid:
                    string guidPattern = "pattern = \"^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$\"";
                    return
                        "        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"            <input matInput {requiredLine} {guidPattern} placeholder=\"e.g. 00000000-0000-0000-0000-000000000000\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}Create.{options.PropertyName.LowerFirstChar()}\">\n" +
                        "        </mat-form-field>";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}