using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlToPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntityUpdatePageHtmlToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");

            stringEditor.InsertLine(GetLine(relationSide));

            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }

        private string GetLine(RelationSide relationSide)
        {
            return
                $"            <app-search-dropdown [formGroupInstance]=\"{relationSide.Entity.NameLower}UpdateForm\"\n" +
                $"                [formControlNameInstance]=\"'{relationSide.NameLower}Id'\" label=\"{relationSide.Name.ToReadable()}\" idExpr=\"id\" displayExpr=\"bezeichnung\"\n" +
                $"                " + ((!relationSide.IsOptional) ? "required=\"true\" " : "") +
                $"[dataSource]=\"{relationSide.NameLower}DataSource\" [initialItem]=\"selected{relationSide.Name}\">\n" +
                $"            </app-search-dropdown>";
        }
    }
}