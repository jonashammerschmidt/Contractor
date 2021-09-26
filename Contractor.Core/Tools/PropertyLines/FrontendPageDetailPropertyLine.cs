using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendPageDetailPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.Boolean:
                    return
                        $"    <p [attr.aria-label]=\"'{options.PropertyName.ToReadable()}: ' + ({options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}) ? 'aktiv' : 'inaktiv' \">\n" +
                        $"        <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{options.PropertyName.ToReadable()}:</span>\n" +
                        $"        <br>\n" +
                        $"        <mat-icon color=\"accent\" *ngIf=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"            check_box\n" +
                        $"        </mat-icon>\n" +
                        $"        <mat-icon style=\"color: gray\" *ngIf=\"!{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"            check_box_outline_blank\n" +
                        $"        </mat-icon>\n" +
                         "    </p>";
                case PropertyTypes.DateTime:
                    return
                        $"    <p [attr.aria-label]=\"'{options.PropertyName.ToReadable()}: ' + {options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}{(options.IsOptional ? "?" : "")}.toLocaleString()\">\n" +
                        $"        <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{options.PropertyName.ToReadable()}:</span>\n" +
                        $"        <br>\n" +
                        $"        <span *ngIf=\"!{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">-</span>\n" +
                        $"        <span *ngIf=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\" aria-hidden=\"true\">{{{{{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()} | date:'dd. MMM. yyyy, HH:mm'}}}}</span>\n" +
                         "    </p>";
                default:
                    return
                        $"    <p [attr.aria-label]=\"'{options.PropertyName.ToReadable()}: ' + {options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                        $"        <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{options.PropertyName.ToReadable()}:</span>\n" +
                        $"        <br>\n" +
                        $"        <span *ngIf=\"!{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">-</span>\n" +
                        $"        <span *ngIf=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\" aria-hidden=\"true\">{{{{{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}}}}}</span>\n" +
                         "    </p>";
            }
        }
    }
}