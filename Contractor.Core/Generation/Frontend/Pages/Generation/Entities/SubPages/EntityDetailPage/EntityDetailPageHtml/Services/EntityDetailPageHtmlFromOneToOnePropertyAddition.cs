using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityDetailPageHtmlFromOneToOnePropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityDetailPageHtmlFromOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
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
                $"    <p *ngIf=\"{relationSide.Entity.NameLower}.{relationSide.NameLower}\">\n" +
                $"        <span style=\"font-size: 0.8em;\">{relationSide.Name.ToReadable()}:</span>\n" +
                $"        <br>\n" +
                $"        <a [routerLink]=\"['/{relationSide.Entity.Module.NameKebab}/{relationSide.OtherEntity.NamePlural}/detail', {relationSide.Entity.NameLower}.{relationSide.NameLower}.id]\">\n" +
                $"            {{{{{relationSide.Entity.NameLower}.{relationSide.NameLower}.{relationSide.OtherEntity.DisplayProperty.NameLower}}}}}\n" +
                $"            <mat-icon style=\"font-size: 1em;\">open_in_new</mat-icon>\n" +
                $"        </a>\n" +
                $"    </p>\n";
        }
    }
}