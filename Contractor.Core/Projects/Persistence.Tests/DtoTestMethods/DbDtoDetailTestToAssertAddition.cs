using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class DbDtoDetailTestToAssertAddition
    {
        public PathService pathService;

        public DbDtoDetailTestToAssertAddition(PathService pathService)
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
            var entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entity", entityOptions.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- AssertDbDefault -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            Db{options.EntityNameFrom}Test.AssertDbDefault(db{options.EntityNameTo}Detail.{options.EntityNameFrom});");

            return stringEditor.GetText();
        }
    }
}