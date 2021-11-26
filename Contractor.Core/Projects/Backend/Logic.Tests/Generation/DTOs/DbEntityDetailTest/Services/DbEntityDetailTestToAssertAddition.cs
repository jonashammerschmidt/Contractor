using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityDetailTestToAssertAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbEntityDetailTestToAssertAddition(
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
            var entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForBackend(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entity", entityOptions.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Creators -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityNameTo}Detail Default()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameFrom} = Db{options.EntityNameFrom}Test.Default(),");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityNameTo}Detail Default2()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameFrom} = Db{options.EntityNameFrom}Test.Default2(),");
            fileData = stringEditor.GetText();

            // ----------- Asserts -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDefault(db{options.EntityNameTo}Detail.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDefault2(db{options.EntityNameTo}Detail.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}