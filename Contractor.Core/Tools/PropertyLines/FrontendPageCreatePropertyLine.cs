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
                        $"            <mat-checkbox formControlName=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"                {options.PropertyName.ToReadable()}\n" +
                        $"            </mat-checkbox>\n" +
                        $"\n" +
                        $"            <br>";

                case PropertyTypes.DateTime:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{options.PropertyName.LowerFirstChar()}\" {requiredLine} placeholder=\"{options.PropertyName.ToReadable()}\" [matDatepicker]=\"picker{options.PropertyName}\">\n" +
                        $"                <mat-datepicker-toggle matSuffix [for]=\"picker{options.PropertyName}\"></mat-datepicker-toggle>\n" +
                        $"                <mat-datepicker #picker{options.PropertyName}></mat-datepicker>\n" +
                        $"                <mat-error *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.touched && {options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.required\">\n" +
                         "                        Dieses Feld ist erforderlich.\n" +
                         "                    </span>\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.pattern\">\n" +
                         "                        Dieses Feld ist ungültig.\n" + 
                         "                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                case PropertyTypes.Integer:
                case PropertyTypes.Double:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{options.PropertyName.LowerFirstChar()}\" type=\"number\" {requiredLine} placeholder=\"{options.PropertyName.ToReadable()}\">\n" +
                        $"                <mat-error *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.touched && {options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.required\">\n" +
                        $"                        Dieses Feld ist erforderlich.\n" +
                        $"                    </span>\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.pattern\">\n" +
                        $"                        Dieses Feld ist ungültig.\n" +
                        $"                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                case PropertyTypes.String:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{options.PropertyName.LowerFirstChar()}\" {requiredLine} maxlength=\"{options.PropertyTypeExtra}\" placeholder=\"{options.PropertyName.ToReadable()}\">\n" +
                        $"                <mat-hint [align]=\"'end'\">{{{{{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.value.length}}}} / {options.PropertyTypeExtra}</mat-hint>\n" +
                        $"                <mat-error *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.touched && {options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.required\">\n" +
                         "                        Dieses Feld ist erforderlich.\n" +
                         "                    </span>\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.pattern\">\n" +
                         "                        Dieses Feld ist ungültig.\n" + 
                         "                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                case PropertyTypes.Guid:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{options.PropertyName.LowerFirstChar()}\" {requiredLine} placeholder=\"z.B. 00000000-0000-0000-0000-000000000000\">\n" +
                        $"                <mat-error *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.touched && {options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.required\">\n" +
                        $"                        Dieses Feld ist erforderlich.\n" +
                        $"                    </span>\n" +
                        $"                    <span *ngIf=\"{options.EntityNameLower}CreateForm.controls.{options.PropertyName.LowerFirstChar()}.errors.pattern\">\n" +
                        $"                        Dieses Feld ist ungültig. Beispiel: 00000000-0000-0000-0000-000000000000.\n" +
                        $"                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}