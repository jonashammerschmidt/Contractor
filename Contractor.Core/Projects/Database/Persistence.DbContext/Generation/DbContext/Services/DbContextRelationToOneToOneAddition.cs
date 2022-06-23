﻿using Contractor.Core.Helpers;
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
            stringEditor.InsertLine($"                entity.Property(e => e.{relationSide.Name}Id)");
            stringEditor.InsertLine($"                    .IsRequired({(!relationSide.IsOptional).ToString().ToLower()});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"                entity.HasOne(d => d.{relationSide.Name})");

            if (relationSide.OtherEntity.Skip)
            {
                stringEditor.InsertLine($"                    .WithOne()");
            }
            else
            {
                stringEditor.InsertLine($"                    .WithOne(p => p.{relationSide.OtherName})");
            }

            if (relationSide.OtherEntity.HasScope && relationSide.Entity.HasScope)
            {
                stringEditor.InsertLine($"                    .HasForeignKey<Ef{relationSide.Entity.Name}>(d => new {{ d.{relationSide.OtherEntity.ScopeEntity.Name}Id, d.{relationSide.Name}Id }})");
            }
            else if (relationSide.OtherEntity.HasScope && !relationSide.Entity.HasScope)
            {
                stringEditor.InsertLine($"                    .HasForeignKey<Ef{relationSide.Entity.Name}>(d => new {{ d.{relationSide.Name}{relationSide.OtherEntity.ScopeEntity.Name}Id, d.{relationSide.Name}Id }})");
            }
            else
            {
                stringEditor.InsertLine($"                    .HasForeignKey<Ef{relationSide.Entity.Name}>(d => d.{relationSide.Name}Id)");
            }

            stringEditor.InsertLine($"                    .IsRequired({(!relationSide.IsOptional).ToString().ToLower()})");
            stringEditor.InsertLine($"                    .OnDelete(DeleteBehavior.{relationSide.OnDelete})");
            stringEditor.InsertLine($"                    .HasConstraintName(\"FK_{relationSide.Entity.NamePlural}_{relationSide.Name}Id\");");

            return stringEditor.GetText();
        }
    }
}