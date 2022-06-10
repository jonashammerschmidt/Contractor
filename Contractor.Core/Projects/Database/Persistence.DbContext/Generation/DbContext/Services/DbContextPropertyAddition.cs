using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class DbContextPropertyAddition : DbContextPropertyAdditionEditor
    {
        public DbContextPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            if (DatabaseDbContextPropertyLine.GetPropertyLine(property) == null)
            {
                return fileData;
            }

            StringEditor stringEditor = new StringEditor(fileData);

            //stringEditor.NextThatContains($"modelBuilder.Entity<Ef{property.Entity.Name}>");

            //if (options.HasClusteredIndex)
            //{
            //    if (!stringEditor.ContainsInTheNextNLines(".HasKey(c => c.Id)", 7))
            //    {
            //        stringEditor.NextThatContains($"entity.ToTable(");
            //        stringEditor.Next();
            //        stringEditor.InsertNewLine();
            //        stringEditor.InsertLine(
            //            $"                entity\n" +
            //            $"                    .HasKey(c => c.Id)\n" +
            //            $"                    .IsClustered(false);");
            //        stringEditor.Next();
            //    }
            //    else
            //    {
            //        stringEditor.NextThatContains($"HasKey(c => c.Id)");
            //        stringEditor.Next();
            //        stringEditor.Next();
            //    }
            //}

            //if (options.HasClusteredIndex || options.HasNonClusteredIndex)
            //{
            //    if (!stringEditor.ContainsInTheNextNLines($".IsClustered({options.HasClusteredIndex.ToString().ToLower()})", 7))
            //    {
            //        stringEditor.InsertLine(
            //            $"                entity\n" +
            //            $"                    .HasIndex(c => c.{property.Name})\n" +
            //            $"                    .IsUnique({options.IsUnique.ToString().ToLower()})\n" +
            //            $"                    .IsClustered({options.HasClusteredIndex.ToString().ToLower()});");
            //        stringEditor.InsertNewLine();
            //    }
            //    else
            //    {
            //        stringEditor.NextThatContains($".IsClustered({options.HasClusteredIndex.ToString().ToLower()})");
            //        stringEditor.Prev();
            //        stringEditor.Prev();

            //        string line = stringEditor.GetLine();

            //        if (line.Contains(".HasIndex(c => new {"))
            //        {
            //            string[] parts = line.Split(new string[] { " }" }, StringSplitOptions.None);
            //            line = $"{parts[0]}, c.{property.Name} }})";
            //        }
            //        else if (line.Contains(".HasIndex(c => c"))
            //        {
            //            string[] parts = line.Split(new string[] { "c => " }, StringSplitOptions.None);
            //            line = $"{parts[0]}c => new {{ {parts[1]}";

            //            parts = line.Split(')');
            //            line = $"{parts[0]}, c.{property.Name} }})";
            //        }

            //        stringEditor.SetLine(line);
            //    }
            //}

            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(DatabaseDbContextPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}