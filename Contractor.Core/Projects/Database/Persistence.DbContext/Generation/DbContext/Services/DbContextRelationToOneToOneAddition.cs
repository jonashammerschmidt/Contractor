using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class DbContextRelationToOneToOneAddition : DbContextRelationAdditionEditor
    {
        public DbContextRelationToOneToOneAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{relationSide.Entity.Name}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(
                    $"                entity.HasOne(d => d.{relationSide.Name})\n" +
                    $"                    .WithOne(p => p.{relationSide.OtherName})\n" +
                    $"                    .HasForeignKey<Ef{relationSide.Entity.Name}>(d => d.{relationSide.Name}Id)\n" +
                    $"                    .IsRequired({(!relationSide.IsOptional).ToString().ToLower()})\n" +
                    $"                    .OnDelete(DeleteBehavior.{relationSide.OnDelete})\n" +
                    $"                    .HasConstraintName(\"FK_{relationSide.Entity.NamePlural}_{relationSide.Name}\");");

            return stringEditor.GetText();
        }
    }
}