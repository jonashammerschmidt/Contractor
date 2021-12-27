using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlToPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntityDetailPageHtmlToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------

            if (fileData.Contains("</mat-tab-group>"))
            {
                stringEditor.NextThatContains("<mat-tab label=\"Stammdaten\">");
                stringEditor.NextThatContains("</mat-tab>");
            }
            else
            {
                stringEditor.NextThatStartsWith($"<div class=\"{options.EntityNameTo.ToKebab()}-detail-page\"");
                stringEditor.NextThatStartsWith($"</div>");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return
                $"    <p>\n" +
                $"        <span style=\"font-size: 0.8em;\">{options.PropertyNameFrom.ToReadable()}:</span>\n" +
                $"        <br>\n" +
                $"        <span *ngIf=\"!{options.EntityNameLowerTo}.{options.PropertyNameFrom.LowerFirstChar()}\">-</span>\n" +
                $"        <a *ngIf=\"{options.EntityNameLowerTo}.{options.PropertyNameFrom.LowerFirstChar()}\"\n" +
                $"            [routerLink]=\"['/{StringConverter.PascalToKebabCase(options.DomainFrom)}/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}/detail', {options.EntityNameLowerTo}.{options.PropertyNameFrom.LowerFirstChar()}.id]\">\n" +
                $"            {{{{{options.EntityNameLowerTo}.{options.PropertyNameFrom.LowerFirstChar()}.bezeichnung}}}}\n" +
                $"            <mat-icon style=\"font-size: 1em;\">open_in_new</mat-icon>\n" +
                $"        </a>\n" +
                $"    </p>\n";
        }
    }
}