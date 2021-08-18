using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityListItemTestToAssertAddition
    {
        public PathService pathService;

        public EntityListItemTestToAssertAddition(PathService pathService)
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

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- AssertDbDefault -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            {options.EntityNameFrom}Test.AssertDefault({options.EntityNameLowerTo}ListItem.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault2(");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            {options.EntityNameFrom}Test.AssertDefault2({options.EntityNameLowerTo}ListItem.{options.PropertyNameFrom});");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}