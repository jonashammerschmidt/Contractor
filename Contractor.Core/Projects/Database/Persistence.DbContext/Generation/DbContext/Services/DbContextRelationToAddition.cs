﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class DbContextRelationToAddition : DbContextRelationAdditionEditor
    {
        public DbContextRelationToAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityNameTo}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(
                    $"                entity.HasOne(d => d.{options.PropertyNameFrom})\n" +
                    $"                    .WithMany(p => p.{options.PropertyNameTo})\n" +
                    $"                    .HasForeignKey(d => d.{options.PropertyNameFrom}Id)\n" +
                    $"                    .IsRequired({(!options.IsOptional).ToString().ToLower()})\n" +
                    $"                    .OnDelete(DeleteBehavior.NoAction)\n" +
                    $"                    .HasConstraintName(\"FK_{options.EntityNamePluralTo}_{options.PropertyNameFrom}Id\");");

            return stringEditor.GetText();
        }
    }
}