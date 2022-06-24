using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class DbContextRelationToAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public DbContextRelationToAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{relationSide.Entity.Name}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();

            stringEditor.InsertLine($"                entity.Property(e => e.{relationSide.Name}Id)");
            stringEditor.InsertLine($"                    .IsRequired({(!relationSide.IsOptional).ToString().ToLower()});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"                entity.HasOne(d => d.{relationSide.Name})");

            if (relationSide.OtherEntity.Skip)
            {
                stringEditor.InsertLine($"                    .WithMany()");
            }
            else
            {
                stringEditor.InsertLine($"                    .WithMany(p => p.{relationSide.OtherName})");
            }


            if (relationSide.OtherEntity.HasScope && relationSide.Entity.HasScope)
            {
                stringEditor.InsertLine($"                    .HasForeignKey(d => new {{ d.{relationSide.OtherEntity.ScopeEntity.Name}Id, d.{relationSide.Name}Id }})");
            }
            else if (relationSide.OtherEntity.HasScope && !relationSide.Entity.HasScope)
            {
                stringEditor.InsertLine($"                    .HasForeignKey(d => new {{ d.{relationSide.Name}{relationSide.OtherEntity.ScopeEntity.Name}Id, d.{relationSide.Name}Id }})");
            }
            else
            {
                stringEditor.InsertLine($"                    .HasForeignKey(d => d.{relationSide.Name}Id)");
            }

            stringEditor.InsertLine($"                    .IsRequired({(!relationSide.IsOptional).ToString().ToLower()})");
            stringEditor.InsertLine($"                    .OnDelete(DeleteBehavior.{relationSide.OnDelete})");
            stringEditor.InsertLine($"                    .HasConstraintName(\"FK_{relationSide.Entity.NamePlural}_{relationSide.Name}Id\");");

            return stringEditor.GetText();
        }
    }
}