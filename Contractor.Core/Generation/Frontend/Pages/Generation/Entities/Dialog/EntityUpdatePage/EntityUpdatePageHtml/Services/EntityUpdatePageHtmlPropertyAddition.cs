﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityUpdatePageHtmlPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityUpdatePageHtmlPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("</form>");

            stringEditor.InsertLine(FrontendPageUpdatePropertyLine.GetPropertyLine(property));

            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }
    }
}