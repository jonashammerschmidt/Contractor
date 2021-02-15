using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class LogicTestsRelationAddition
    {
        public PathService pathService;

        public LogicTestsRelationAddition(PathService pathService)
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
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Model.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Property -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"{options.EntityNamePluralFrom}CrudLogic {options.EntityNamePluralLowerFrom}CrudLogic = new {options.EntityNamePluralFrom}CrudLogic");
            while (stringEditor.GetLineNumber() < stringEditor.GetLineCount())
            {
                stringEditor.Next(line => !line.Contains("CrudRepository.Object"));
                stringEditor.InsertLine("                null,");
                stringEditor.NextThatContains($"{options.EntityNamePluralFrom}CrudLogic {options.EntityNamePluralLowerFrom}CrudLogic = new {options.EntityNamePluralFrom}CrudLogic");
            }

            return stringEditor.GetText();
        }
    }
}