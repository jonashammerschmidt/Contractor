using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendPageCreateUpdatePropertyLine
    {
        public enum FormType
        {
            CreateDialog,
            DetailPage,
        }

        public static string GetPropertyLine(Property property, FormType formType)
        {
            string formTypeString = formType.ToString().ToKebab();
            switch (property.Type)
            {
                case PropertyType.Boolean:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                        $"                    <div class=\"form-layout-inputs\">\n" +
                        $"                        <mat-checkbox formControlName=\"{property.Name.LowerFirstChar()}\"\n" +
                        $"                            data-testid=\"{property.Entity.Module.NameKebab}-{property.Entity.NameKebab}-{formTypeString}-{property.Name.ToKebab()}-checkbox\"></mat-checkbox>\n" +
                        $"                    </div>\n" +
                        $"                </div>";

                case PropertyType.ByteArray:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                         "                            <mat-icon matSuffix>upload_file</mat-icon>\n" +
                        $"                            <file-picker formControlName=\"{property.Name.LowerFirstChar()}\" placeholder=\"{property.Name.ToReadable()}\"\n" +
                        $"                                data-testid=\"{property.Entity.Module.NameKebab}-{property.Entity.NameKebab}-{formTypeString}-{property.Name.ToKebab()}-file-picker\"></file-picker>\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </form-layout-row-status>\n" +
                         "                </div>";

                case PropertyType.DateTime:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" placeholder =\"{property.Name.ToReadable()}\"\n" +
                        $"                                [matDatepicker]=\"picker{property.Name}\"\n" +
                        $"                                data-testid=\"{property.Entity.Module.NameKebab}-{property.Entity.NameKebab}-{formTypeString}-{property.Name.ToKebab()}-date-input\">\n" +
                        $"                            <mat-datepicker-toggle matIconSuffix [for]=\"picker{property.Name}\">\n" +
                         "                            </mat-datepicker-toggle>\n" +
                        $"                            <mat-datepicker #picker{property.Name}></mat-datepicker>\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </form-layout-row-status>\n" +
                         "                </div>";

                case PropertyType.Integer:
                case PropertyType.Double:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\"\n" +
                        $"                                type=\"number\" placeholder=\"{property.Name.ToReadable()}\"\n" +
                        $"                                data-testid=\"{property.Entity.Module.NameKebab}-{property.Entity.NameKebab}-{formTypeString}-{property.Name.ToKebab()}-input\">\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </form-layout-row-status>\n" +
                         "                </div>";

                case PropertyType.String:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" maxlength=\"{property.TypeExtra}\"\n" +
                        $"                                placeholder=\"{property.Name.ToReadable()}\" data-testid=\"{property.Entity.Module.NameKebab}-{property.Entity.NameKebab}-{formTypeString}-{property.Name.ToKebab()}-input\">\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </form-layout-row-status>\n" +
                         "                </div>";

                case PropertyType.Guid:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" maxlength=\"36\"\n" +
                        $"                                placeholder=\"z.B. 00000000-0000-0000-0000-000000000000\"\n" +
                        $"                                data-testid=\"{property.Entity.Module.NameKebab}-{property.Entity.NameKebab}-{formTypeString}-{property.Name.ToKebab()}-input\">\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"pattern\"\n" +
                         "                            title=\"Eingabe ungültig\" description=\"Dieses Feld ist ungültig. Beispiel: 00000000-0000-0000-0000-000000000000\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </form-layout-row-status>\n" +
                         "                </div>";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}