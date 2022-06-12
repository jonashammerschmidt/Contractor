using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlToPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntityDetailPageHtmlToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            if (fileData.Contains("</mat-tab-group>"))
            {
                stringEditor.NextThatContains("<mat-tab label=\"Stammdaten\">");
                stringEditor.NextThatContains("</mat-tab>");
            }
            else
            {
                stringEditor.NextThatStartsWith($"<div class=\"{relationSide.Entity.NameKebab}-detail-page\"");
                stringEditor.NextThatStartsWith($"</div>");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(relationSide));

            return stringEditor.GetText();
        }

        private string GetLine(RelationSide relationSide)
        {
            return
                $"            <p>\n" +
                $"                <span style=\"font-size: 0.8em;\">{relationSide.NameLower.ToReadable()}:</span>\n" +
                $"                <br>\n" +
                $"                <span *ngIf=\"!{relationSide.Entity.NameLower}.{relationSide.NameLower}\">-</span>\n" +
                $"                <a *ngIf=\"{relationSide.Entity.NameLower}.{relationSide.NameLower}\"\n" +
                $"                    [routerLink]=\"['/{relationSide.OtherEntity.Module.NameKebab}/{relationSide.OtherEntity.NamePluralKebab}/detail', {relationSide.Entity.NameLower}.{relationSide.NameLower}.id]\">\n" +
                $"                    {{{{{relationSide.Entity.NameLower}.{relationSide.NameLower}.{relationSide.OtherEntity.DisplayProperty.NameLower}}}}}\n" +
                $"                    <mat-icon style=\"font-size: 1em;\">open_in_new</mat-icon>\n" +
                $"                </a>\n" +
                $"            </p>\n";
        }
    }
}