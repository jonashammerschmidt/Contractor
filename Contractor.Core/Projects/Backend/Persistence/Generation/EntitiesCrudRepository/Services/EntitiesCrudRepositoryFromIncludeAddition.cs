using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryFromIncludeAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesCrudRepositoryFromIncludeAddition(
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

            this.fileSystemClient.WriteAllText(filePath, fileData, options);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForFrom(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"Get{options.EntityNameFrom}Detail(");
            stringEditor.NextThatContains($"this.dbContext.{options.EntityNamePluralFrom}");
            stringEditor.Next(line => !line.Contains("Include("));

            stringEditor.InsertLine($"                .Include(ef{options.EntityNameFrom} => ef{options.EntityNameFrom}.{options.PropertyNameTo})");

            return stringEditor.GetText();
        }
    }
}