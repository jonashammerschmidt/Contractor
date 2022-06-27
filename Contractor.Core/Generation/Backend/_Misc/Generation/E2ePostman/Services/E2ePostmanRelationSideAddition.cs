using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.Linq;

namespace Contractor.Core.Generation.Backend.Misc
{
    internal class E2ePostmanRelationSideAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public E2ePostmanRelationSideAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatStartsWith("\t\"item\": [");
            stringEditor.NextThatStartsWith($"\t\t\t\"name\": \"{relationSide.Entity.Module.Name} - {relationSide.Entity.NamePlural}\",");

            InsertCreateLine(relationSide, stringEditor, string.Empty, "1", false);

            InsertUpdateLine(relationSide, stringEditor);

            InsertCreateLine(relationSide, stringEditor, " 1", "1", false);

            InsertCreateLine(relationSide, stringEditor, " 2", "2", relationSide.IsOptional);

            return stringEditor.GetText();
        }

        private static void InsertCreateLine(RelationSide relationSide, StringEditor stringEditor, string postfix1, string postfix2, bool isOptional)
        {
            stringEditor.NextThatStartsWith($"\t\t\t\t\t\"name\": \"Create {relationSide.Entity.Name + postfix1}\",");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\"body\": {");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\t\"raw\": \"{\\n");
            if (stringEditor.GetLine().Split(':').Count() > 2)
            {
                stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $",");
            }

            if (isOptional)
            {
                stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $"\\n  \\\"{relationSide.NameLower}Id\\\": null");
            }
            else
            {
                stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $"\\n  \\\"{relationSide.NameLower}Id\\\": \\\"{{{{e2e{relationSide.OtherEntity.Name}{postfix2}Id}}}}\\\"");
            }
        }

        private static void InsertUpdateLine(RelationSide relationSide, StringEditor stringEditor)
        {
            stringEditor.NextThatStartsWith($"\t\t\t\t\t\"name\": \"Update {relationSide.Entity.Name}\",");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\"body\": {");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\t\"raw\": \"{\\n");

            stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $",\\n  \\\"{relationSide.NameLower}Id\\\": \\\"{{{{e2e{relationSide.OtherEntity.Name}2Id}}}}\\\"");
        }
    }
}