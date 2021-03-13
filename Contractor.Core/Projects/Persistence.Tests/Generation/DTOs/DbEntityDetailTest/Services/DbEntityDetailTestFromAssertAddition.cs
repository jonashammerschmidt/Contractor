using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence.Tests
{
    internal class DbEntityDetailTestFromAssertAddition
    {
        public PathService pathService;

        public DbEntityDetailTestFromAssertAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
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
            string fileData = File.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, "System.Linq");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // ----------- AssertDbDefault -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            Db{options.EntityNameTo}Test.AssertDbDefault(db{options.EntityNameFrom}Detail.{options.EntityNamePluralTo}.ToArray()[0]);");

            return stringEditor.GetText();
        }
    }
}