using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryToOneToOneIncludeAddition : RelationAdditionEditor
    {
        public EntitiesCrudRepositoryToOneToOneIncludeAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "Microsoft.EntityFrameworkCore");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"Get{relationSide.Entity.Name}Detail(");
            stringEditor.NextThatContains($"this.dbContext.{relationSide.Entity.NamePlural}");
            stringEditor.Next(line => !line.Contains("Include("));
            stringEditor.InsertLine($"                .Include(ef{relationSide.Entity.Name} => ef{relationSide.Entity.Name}.{relationSide.Name})");
            stringEditor.MoveToStart();

            string includeLine = $"                .Include(ef{relationSide.Entity.Name} => ef{relationSide.Entity.Name}.{relationSide.Name})";
            stringEditor.NextThatContains($"GetPaged{relationSide.Entity.NamePlural}(");
            stringEditor.NextThatContains($"this.dbContext.{relationSide.Entity.NamePlural}");
            stringEditor.Next(line => !line.Contains("Include("));
            stringEditor.Prev();
            if (stringEditor.GetLine().Contains(";"))
            {
                stringEditor.SetLine(stringEditor.GetLine().Replace(";", ""));
            }
            stringEditor.Next();

            if (!stringEditor.GetLine().Trim().StartsWith("."))
            {
                includeLine += ";";
            }
            stringEditor.InsertLine(includeLine);

            stringEditor.MoveToStart();

            stringEditor.NextThatContains($"public bool Does{relationSide.Entity.Name}Exist(");
            stringEditor.Next(line => line.StartsWith("        }"));
            stringEditor.Next();
            stringEditor.InsertNewLine();

            stringEditor.InsertLine(
                $"        public bool Is{relationSide.Name}InUse(Guid {relationSide.NameLower})\n" +
                 "        {\n" +
                $"            return this.dbContext.{relationSide.Entity.NamePlural}.Any(ef{relationSide.Entity.Name} => ef{relationSide.Entity.Name}.{relationSide.Name} == {relationSide.NameLower});\n" +
                 "        }");

            return stringEditor.GetText();
        }
    }
}