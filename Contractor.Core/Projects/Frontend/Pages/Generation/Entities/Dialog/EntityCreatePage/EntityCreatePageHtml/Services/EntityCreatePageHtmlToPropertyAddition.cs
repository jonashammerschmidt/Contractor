using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageHtmlToPropertyAddition : RelationAdditionEditor
    {
        public EntityCreatePageHtmlToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("</form>");

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return
              $"            <app-search-dropdown [formGroupInstance]=\"{options.EntityNameLowerTo}CreateForm\" [formControlNameInstance]=\"'{options.PropertyNameFrom.LowerFirstChar()}Id'\"\n" +
              $"                label=\"{options.PropertyNameFrom.ToReadable()}\" idExpr=\"id\" displayExpr=\"bezeichnung\"" +
              ((!options.IsOptional) ? " required=\"true\"" : "") +
              $" [dataSource]=\"{options.PropertyNameFrom.LowerFirstChar()}DataSource\"></app-search-dropdown>";
        }
    }
}