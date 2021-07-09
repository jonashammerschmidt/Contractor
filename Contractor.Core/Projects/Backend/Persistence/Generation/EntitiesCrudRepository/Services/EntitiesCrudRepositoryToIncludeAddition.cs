using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryToIncludeAddition
    {
        public PathService pathService;

        public EntitiesCrudRepositoryToIncludeAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = UsingStatements.Add(fileData, "Microsoft.EntityFrameworkCore");

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"Get{options.EntityNameTo}Detail(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralTo}");
            stringEditor.Next(line => !line.Contains("Include("));
            stringEditor.InsertLine($"                .Include(ef{options.EntityNameTo} => ef{options.EntityNameTo}.{options.PropertyNameFrom})");
            stringEditor.MoveToStart();

            stringEditor.NextThatContains($"GetPaged{options.EntityNamePluralTo}(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralTo}");
            stringEditor.Next(line => !line.Contains("Include("));
            stringEditor.Prev();
            if (stringEditor.GetLine().Contains(";"))
            {
                stringEditor.SetLine(stringEditor.GetLine().Replace(";", ""));
            }
            stringEditor.Next();
            stringEditor.InsertLine($"                .Include(ef{options.EntityNameTo} => ef{options.EntityNameTo}.{options.PropertyNameFrom});");
            
            return stringEditor.GetText();
        }
    }
}