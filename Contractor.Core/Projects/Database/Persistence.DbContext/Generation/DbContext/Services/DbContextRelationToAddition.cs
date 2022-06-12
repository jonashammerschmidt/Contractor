using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class DbContextRelationToAddition : DbContextRelationAdditionEditor
    {
        public DbContextRelationToAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = InsertEmptyModelBuilderEntityLine(relationSide, fileData);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{relationSide.Entity.Name}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertLine(
                    $"                entity.HasOne(d => d.{relationSide.Name})\n" +
                    $"                    .WithMany(p => p.{relationSide.OtherName})\n" +
                    $"                    .HasForeignKey(d => d.{relationSide.Name}Id)\n" +
                    $"                    .IsRequired({(!relationSide.IsOptional).ToString().ToLower()})\n" +
                    $"                    .OnDelete(DeleteBehavior.NoAction)\n" +
                    $"                    .HasConstraintName(\"FK_{relationSide.Entity.NamePlural}_{relationSide.Name}\");");

            return stringEditor.GetText();
        }

        private string InsertEmptyModelBuilderEntityLine(RelationSide relationSide, string fileData)
        {
            if (fileData.Contains($"modelBuilder.Entity<Ef{relationSide.Entity.Name}>"))
            {
                StringEditor stringEditorForLineInsert = new StringEditor(fileData);

                stringEditorForLineInsert.NextThatContains($"modelBuilder.Entity<Ef{relationSide.Entity.Name}>");
                stringEditorForLineInsert.NextThatContains("});");

                stringEditorForLineInsert.InsertNewLine();
                return stringEditorForLineInsert.GetText();
            }

            string usingStatement = $"{relationSide.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSide.Entity.Module.Name}.{relationSide.Entity.NamePlural}";
            fileData = UsingStatements.Add(fileData, usingStatement);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("this.OnModelCreatingPartial(modelBuilder);");

            stringEditor.InsertLine(
                $"            modelBuilder.Entity<Ef{relationSide.Entity.Name}>(entity =>\n" +
                 "            {\n" +
                 "            });");
            stringEditor.InsertNewLine();
            stringEditor.MoveToStart();

            return stringEditor.GetText();
        }
    }
}