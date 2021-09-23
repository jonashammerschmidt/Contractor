using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityListItemTestFromAssertAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbEntityListItemTestFromAssertAddition(
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

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            var entityOptions = RelationAdditionOptions.GetPropertyForFrom(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entity", entityOptions.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // ----------- Creators -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityNameFrom}ListItem Default()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameTo} = Db{options.EntityNameTo}Test.Default(),");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityNameFrom}ListItem Default2()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameTo} = Db{options.EntityNameTo}Test.Default2(),");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}