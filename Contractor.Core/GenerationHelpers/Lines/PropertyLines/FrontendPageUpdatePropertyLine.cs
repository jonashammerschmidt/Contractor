using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendPageUpdatePropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            string requiredLine = (property.IsOptional ? "" : "required=\"true\"");
            switch (property.Type)
            {
                case PropertyType.Boolean:
                    return
                        $"            <mat-checkbox formControlName=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                {property.Name.ToReadable()}\n" +
                        $"            </mat-checkbox>\n" +
                        $"\n" +
                        $"            <br>";

                case PropertyType.DateTime:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{property.Name.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" {requiredLine} placeholder=\"{property.Name.ToReadable()}\" [matDatepicker]=\"picker{property.Name}\">\n" +
                        $"                <mat-datepicker-toggle matSuffix [for]=\"picker{property.Name}\"></mat-datepicker-toggle>\n" +
                        $"                <mat-datepicker #picker{property.Name}></mat-datepicker>\n" +
                        $"                <mat-error *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.touched && {property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.required\">\n" +
                        $"                        Dieses Feld ist erforderlich.\n" +
                        $"                    </span>\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.pattern\">\n" +
                        $"                        Dieses Feld ist ungültig.\n" +
                        $"                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                case PropertyType.Integer:
                case PropertyType.Double:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{property.Name.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" type=\"number\" {requiredLine} placeholder=\"{property.Name.ToReadable()}\">\n" +
                        $"                <mat-error *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.touched && {property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.required\">\n" +
                        $"                        Dieses Feld ist erforderlich.\n" +
                        $"                    </span>\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.pattern\">\n" +
                        $"                        Dieses Feld ist ungültig.\n" +
                        $"                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                case PropertyType.String:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{property.Name.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" {requiredLine} maxlength=\"{property.TypeExtra}\" placeholder=\"{property.Name.ToReadable()}\">\n" +
                        $"                <mat-hint *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.value\" [align]=\"'end'\">\n" +
                        $"                    {{{{{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.value.length}}}} / {property.TypeExtra}\n" +
                        $"                </mat-hint>\n" +
                        $"                <mat-error *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.touched && {property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.required\">\n" +
                        $"                        Dieses Feld ist erforderlich.\n" +
                        $"                    </span>\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.pattern\">\n" +
                        $"                        Dieses Feld ist ungültig.\n" +
                        $"                    </span>\n" +
                         "                </mat-error>\n" +
                         "            </mat-form-field>";

                case PropertyType.Guid:
                    return
                         "            <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                <mat-label>{property.Name.ToReadable()}</mat-label>\n" +
                        $"                <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" {requiredLine} placeholder=\"z.B. 00000000-0000-0000-0000-000000000000\">\n" +
                        $"                <mat-error *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.touched && {property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.invalid\">\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.required\">\n" +
                        $"                        Dieses Feld ist erforderlich.\n" +
                        $"                    </span>\n" +
                        $"                    <span *ngIf=\"{property.Entity.NameLower}UpdateForm.controls.{property.Name.LowerFirstChar()}.errors.pattern\">\n" +
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