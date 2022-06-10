﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageHtmlToOneToOnePropertyAddition : FrontendRelationAdditionEditor
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
              $"                label=\"{relationSide.Name.ToReadable()}\" idExpr=\"id\" displayExpr=\"bezeichnung\" required=\"true\" [dataSource]=\"{relationSide.NameLower}DataSource\"></app-search-dropdown>";
        }
    }
}