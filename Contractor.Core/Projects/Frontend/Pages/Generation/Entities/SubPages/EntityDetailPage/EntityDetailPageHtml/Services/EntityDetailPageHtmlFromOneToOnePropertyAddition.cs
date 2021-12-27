using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlFromOneToOnePropertyAddition : RelationAdditionEditor
    {
        public EntityDetailPageHtmlFromOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
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
                stringEditor.NextThatStartsWith($"<div class=\"{options.EntityNameFrom.ToKebab()}-detail-page\"");
                stringEditor.NextThatStartsWith($"</div>");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return
                $"    <p *ngIf=\"{options.EntityNameLowerFrom}.{options.PropertyNameTo.LowerFirstChar()}\">\n" +
                $"        <span style=\"font-size: 0.8em;\">{options.PropertyNameTo.ToReadable()}:</span>\n" +
                $"        <br>\n" +
                $"        <a [routerLink]=\"['/{StringConverter.PascalToKebabCase(options.DomainTo)}/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}/detail', {options.EntityNameLowerFrom}.{options.PropertyNameTo.LowerFirstChar()}.id]\">\n" +
                $"            {{{{{options.EntityNameLowerFrom}.{options.PropertyNameTo.LowerFirstChar()}.bezeichnung}}}}\n" +
                $"            <mat-icon style=\"font-size: 1em;\">open_in_new</mat-icon>\n" +
                $"        </a>\n" +
                $"    </p>\n";
        }
    }
}