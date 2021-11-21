using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryToOneToOneIncludeAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesCrudRepositoryToOneToOneIncludeAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = UsingStatements.Add(fileData, "Microsoft.EntityFrameworkCore");

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForBackend(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"Get{options.EntityNameTo}Detail(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralTo}");
            stringEditor.Next(line => !line.Contains("Include("));
            stringEditor.InsertLine($"                .Include(ef{options.EntityNameTo} => ef{options.EntityNameTo}.{options.PropertyNameFrom})");
            stringEditor.MoveToStart();

            string includeLine = $"                .Include(ef{options.EntityNameTo} => ef{options.EntityNameTo}.{options.PropertyNameFrom})";
            stringEditor.NextThatContains($"GetPaged{options.EntityNamePluralTo}(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralTo}");
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

            stringEditor.NextThatContains($"public bool Does{options.EntityNameTo}Exist(");
            stringEditor.Next(line => line.StartsWith("        }"));
            stringEditor.Next();
            stringEditor.InsertNewLine();

            stringEditor.InsertLine(
                $"        public bool Is{options.PropertyNameFrom}IdInUse(Guid {options.PropertyNameFrom.LowerFirstChar()}Id)\n" +
                 "        {\n" +
                $"            return this.dbContext.{options.EntityNamePluralTo}.Any(ef{options.EntityNameTo} => ef{options.EntityNameTo}.{options.PropertyNameFrom}Id == {options.PropertyNameFrom.LowerFirstChar()}Id);\n" +
                 "        }");

            return stringEditor.GetText();
        }
    }
}