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

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            if (DatabaseDbContextPropertyLine.GetPropertyLine(options) == null)
            {
                return fileData;
            }

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"modelBuilder.Entity<Ef{options.EntityName}>");

            if (options.HasClusteredIndex)
            {
                if (!stringEditor.ContainsInTheNextNLines(".HasKey(c => c.Id)", 7))
                {
                    stringEditor.NextThatContains($"entity.ToTable(");
                    stringEditor.Next();
                    stringEditor.InsertNewLine();
                    stringEditor.InsertLine(
                        $"                entity\n" +
                        $"                    .HasKey(c => c.Id)\n" +
                        $"                    .IsClustered(false);");
                    stringEditor.Next();
                }
                else
                {
                    stringEditor.NextThatContains($"HasKey(c => c.Id)");
                    stringEditor.Next();
                    stringEditor.Next();
                }
            }

            if (options.HasClusteredIndex || options.HasNonClusteredIndex)
            {
                if (!stringEditor.ContainsInTheNextNLines($".IsClustered({options.HasClusteredIndex.ToString().ToLower()})", 7))
                {
                    stringEditor.InsertLine(
                        $"                entity" +
                        $"                    .HasIndex(c => c.{options.PropertyName})" +
                        $"                    .IsClustered({options.HasClusteredIndex.ToString().ToLower()});");
                }
                else
                {
                    stringEditor.NextThatContains($".IsClustered({options.HasClusteredIndex.ToString().ToLower()})");
                    stringEditor.Prev();

                    string line = stringEditor.GetLine();

                    if (line.Contains(".HasIndex(c => new {"))
                    {
                        string[] parts = line.Split(new string[] { " }" }, StringSplitOptions.None);
                        line = $"{parts[0]}, c.{options.PropertyName} }}";

                        stringEditor.SetLine(line);
                    }
                    else if (line.Contains(".HasIndex(c => c"))
                    {
                        string[] parts = line.Split(new string[] { "c =>" }, StringSplitOptions.None);
                        line = $"{parts[0]} new {{ {parts[1]}";

                        parts = line.Split(')');
                        line = $"{parts[0]} }})";

                    }

                    stringEditor.SetLine(line);
                }
            }

            stringEditor.NextThatContains("});");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(DatabaseDbContextPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}