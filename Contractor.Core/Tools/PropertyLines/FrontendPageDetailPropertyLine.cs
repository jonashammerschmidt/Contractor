using Contractor.Core.Helpers;
using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class FrontendPageDetailPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            switch (property.Type)
            {
                case PropertyTypes.Boolean:
                    return
                        $"    <p [attr.aria-label]=\"'{property.Name.ToReadable()}: ' + ({property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}) ? 'aktiv' : 'inaktiv' \">\n" +
                        $"        <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{property.Name.ToReadable()}:</span>\n" +
                        $"        <br>\n" +
                        $"        <mat-icon color=\"accent\" *ngIf=\"{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\">\n" +
                        $"            check_box\n" +
                        $"        </mat-icon>\n" +
                        $"        <mat-icon style=\"color: gray\" *ngIf=\"!{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\">\n" +
                        $"            check_box_outline_blank\n" +
                        $"        </mat-icon>\n" +
                         "    </p>";
                case PropertyTypes.DateTime:
                    return
                        $"    <p [attr.aria-label]=\"'{property.Name.ToReadable()}: ' + {property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}{(property.IsOptional ? "?" : "")}.toLocaleString()\">\n" +
                        $"        <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{property.Name.ToReadable()}:</span>\n" +
                        $"        <br>\n" +
                        $"        <span *ngIf=\"!{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\">-</span>\n" +
                        $"        <span *ngIf=\"{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\" aria-hidden=\"true\">{{{{{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()} | date:'dd. MMM. yyyy, HH:mm'}}}}</span>\n" +
                         "    </p>";
                default:
                    return
                        $"    <p [attr.aria-label]=\"'{property.Name.ToReadable()}: ' + {property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\">\n" +
                        $"        <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{property.Name.ToReadable()}:</span>\n" +
                        $"        <br>\n" +
                        $"        <span *ngIf=\"!{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\">-</span>\n" +
                        $"        <span *ngIf=\"{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}\" aria-hidden=\"true\">{{{{{property.Entity.Name.LowerFirstChar()}.{property.Name.LowerFirstChar()}}}}}</span>\n" +
                         "    </p>";
            }
        }
    }
}