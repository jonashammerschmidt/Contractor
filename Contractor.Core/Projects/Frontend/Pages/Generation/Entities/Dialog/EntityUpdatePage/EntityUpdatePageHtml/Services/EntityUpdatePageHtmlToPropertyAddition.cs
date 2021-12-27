using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlToPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntityUpdatePageHtmlToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");

            stringEditor.InsertLine(GetLine(options));

            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return
                $"            <app-search-dropdown [formGroupInstance]=\"{options.EntityNameLowerTo}UpdateForm\"\n" +
                $"                [formControlNameInstance]=\"'{options.PropertyNameFrom.LowerFirstChar()}Id'\" label=\"{options.PropertyNameFrom.ToReadable()}\" idExpr=\"id\" displayExpr=\"bezeichnung\"\n" +
                $"                " + 
                 ((!options.IsOptional) ? "required=\"true\" " : "") +
                $"[dataSource]=\"{options.PropertyNameFrom.LowerFirstChar()}DataSource\" [initialItem]=\"selected{options.PropertyNameFrom}\">\n" +
                $"            </app-search-dropdown>";
        }
    }
}