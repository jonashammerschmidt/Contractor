using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendPageCreateUpdatePropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            switch (property.Type)
            {
                case PropertyType.Boolean:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                        $"                    <div class=\"form-layout-inputs\">\n" +
                        $"                        <mat-checkbox formControlName=\"{property.Name.LowerFirstChar()}\"></mat-checkbox>\n" +
                        $"                    </div>\n" +
                        $"                </div>";

                case PropertyType.ByteArray:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                         "                            <mat-icon matSuffix>upload_file</mat-icon>\n" +
                        $"                            <file-picker formControlName=\"{property.Name.LowerFirstChar()}\" placeholder=\"{property.Name.ToReadable()}\"></file-picker>\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status></form-layout-row-status>\n" +
                         "                    <div class=\"form-layout-side-bar\">\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </div>\n" +
                         "                </div>";

                case PropertyType.DateTime:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" placeholder =\"{property.Name.ToReadable()}\"\n" +
                        $"                                [matDatepicker]=\"picker{property.Name}\">\n" +
                        $"                            <mat-datepicker-toggle matIconSuffix [for]=\"picker{property.Name}\">\n" +
                         "                            </mat-datepicker-toggle>\n" +
                        $"                            <mat-datepicker #picker{property.Name}></mat-datepicker>\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status></form-layout-row-status>\n" +
                         "                    <div class=\"form-layout-side-bar\">\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </div>\n" +
                         "                </div>";

                case PropertyType.Integer:
                case PropertyType.Double:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" maxlength=\"50\"\n" +
                        $"                                type=\"number\" placeholder=\"{property.Name.ToReadable()}\">\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status></form-layout-row-status>\n" +
                         "                    <div class=\"form-layout-side-bar\">\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </div>\n" +
                         "                </div>";

                case PropertyType.String:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" maxlength=\"50\"\n" +
                        $"                                placeholder=\"{property.Name.ToReadable()}\">\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status></form-layout-row-status>\n" +
                         "                    <div class=\"form-layout-side-bar\">\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </div>\n" +
                         "                </div>";

                case PropertyType.Guid:
                    return
                        $"                <div formLayoutRow [formGroupInstance]=\"formController.formGroup\" formControlNameInstance=\"{property.Name.LowerFirstChar()}\">\n" +
                        $"                    <mat-label>{property.Name.ToReadable()}:</mat-label>\n" +
                         "                    <div class=\"form-layout-inputs\">\n" +
                         "                        <mat-form-field appearance=\"outline\" floatLabel=\"always\">\n" +
                        $"                            <input matInput formControlName=\"{property.Name.LowerFirstChar()}\" maxlength=\"50\"\n" +
                        $"                                placeholder=\"z.B. 00000000-0000-0000-0000-000000000000\">\n" +
                         "                        </mat-form-field>\n" +
                         "                    </div>\n" +
                         "                    <form-layout-row-status></form-layout-row-status>\n" +
                         "                    <div class=\"form-layout-side-bar\">\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"required\"\n" +
                         "                            title=\"Eingabe fehlt\" description=\"Dieses Feld ist erforderlich.\">\n" +
                         "                        </form-layout-error>\n" +
                        $"                        <form-layout-error formControlNameInstance=\"{property.Name.LowerFirstChar()}\" errorType=\"pattern\"\n" +
                         "                            title=\"Eingabe ungültig\" description=\"Dieses Feld ist ungültig. Beispiel: 00000000-0000-0000-0000-000000000000\">\n" +
                         "                        </form-layout-error>\n" +
                         "                    </div>\n" +
                         "                </div>";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}