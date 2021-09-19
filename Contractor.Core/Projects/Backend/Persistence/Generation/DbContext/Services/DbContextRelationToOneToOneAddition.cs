﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbContextRelationToOneToOneAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbContextRelationToOneToOneAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityNameTo}>");
            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"                entity.HasOne(d => d.{options.PropertyNameFrom})\n" +
                    $"                    .WithOne(p => p.{options.PropertyNameTo})\n" +
                    $"                    .HasForeignKey<Ef{options.EntityNameTo}>(d => d.{options.PropertyNameFrom}Id)\n" +
                    $"                    .HasConstraintName(\"FK_{options.EntityNamePluralTo}_{options.PropertyNameFrom}Id\");");

            return stringEditor.GetText();
        }
    }
}