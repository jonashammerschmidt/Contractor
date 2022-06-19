﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Misc
{
    internal class E2ePostmanEntityAddition : EntityAdditionToExisitingFileGeneration
    {
        public E2ePostmanEntityAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Entity entity, string fileData)
        {
            var e2ePostmanEntityText = this.ReadFile(entity, MiscBackendGeneration.TemplateFolder, "E2ePostmanTemplate.txt");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatStartsWith("\t\"item\": [");
            stringEditor.NextThatStartsWith($"\t\t\t\"name\": \"{entity.Module.Name}\"");
            stringEditor.NextThatStartsWith("\t\t\t]");

            stringEditor.Prev();
            if (!stringEditor.GetLine().Contains("\"item\": ["))
            {
                stringEditor.InsertIntoLine(",");
            }
            stringEditor.Next();

            stringEditor.InsertLine(e2ePostmanEntityText);

            return stringEditor.GetText();
        }
    }
}