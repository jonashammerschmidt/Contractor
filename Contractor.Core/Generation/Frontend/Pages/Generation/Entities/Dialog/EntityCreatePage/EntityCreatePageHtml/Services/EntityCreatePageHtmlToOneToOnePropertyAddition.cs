using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityCreatePageHtmlToOneToOnePropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityCreatePageHtmlToOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(relationSide));

            return stringEditor.GetText();
        }

        private string GetLine(RelationSide relationSide)
        {
            return
              $"            <app-search-dropdown [formGroupInstance]=\"{relationSide.Entity.NameLower}CreateForm\" [formControlNameInstance]=\"'{relationSide.NameLower}Id'\"\n" +
              $"                label=\"{relationSide.Name.ToReadable()}\" idExpr=\"id\" displayExpr=\"{relationSide.OtherEntity.DisplayProperty.NameLower}\" required=\"true\" [dataSource]=\"{relationSide.NameLower}DataSource\"></app-search-dropdown>";
        }
    }
}