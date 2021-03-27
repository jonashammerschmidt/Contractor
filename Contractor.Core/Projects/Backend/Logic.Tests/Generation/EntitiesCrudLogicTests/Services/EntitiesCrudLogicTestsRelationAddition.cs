using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntitiesCrudLogicTestsRelationAddition
    {
        public PathService pathService;

        public EntitiesCrudLogicTestsRelationAddition(PathService pathService)
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

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Repository Generation -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.Next();
            stringEditor.PrevThatContains("}");
            stringEditor.PrevThatContains("}");
            stringEditor.InsertLine("\n" +
                 $"        private Mock<I{options.EntityNamePluralFrom}CrudRepository> Setup{options.EntityNamePluralFrom}RepositoryDefault()\n" +
                  "        {\n" +
                 $"            var {options.EntityNamePluralLowerFrom}CrudRepository = new Mock<I{options.EntityNamePluralFrom}CrudRepository>(MockBehavior.Strict);\n" +
                 $"            {options.EntityNamePluralLowerFrom}CrudRepository.Setup(repository => repository.Does{options.EntityNameFrom}Exist({options.EntityNameFrom}TestValues.IdDefault)).Returns(true);\n" +
                 $"            {options.EntityNamePluralLowerFrom}CrudRepository.Setup(repository => repository.Does{options.EntityNameFrom}Exist({options.EntityNameFrom}TestValues.IdForCreate)).Returns(true);\n" +
                 $"            {options.EntityNamePluralLowerFrom}CrudRepository.Setup(repository => repository.Does{options.EntityNameFrom}Exist({options.EntityNameFrom}TestValues.IdForUpdate)).Returns(true);\n" +
                 $"            return {options.EntityNamePluralLowerFrom}CrudRepository;\n" +
                  "        }");

            fileData = stringEditor.GetText();

            // ----------- TestMethods -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("[TestMethod]");
            while (stringEditor.GetLineNumber() < stringEditor.GetLineCount())
            {
                stringEditor.Next();
                if (stringEditor.GetLine().Contains("Create" + options.EntityNameTo) ||
                    stringEditor.GetLine().Contains("Update" + options.EntityNameTo))
                {
                    stringEditor.NextThatContains($"Mock<I{options.EntityNamePluralTo}CrudRepository>");
                    stringEditor.Next(line => !line.Contains("CrudRepository>") && line.Trim().Length > 0);
                    stringEditor.InsertLine($"            Mock<I{options.EntityNamePluralFrom}CrudRepository> {options.EntityNamePluralLowerFrom}CrudRepository = this.Setup{options.EntityNamePluralFrom}RepositoryDefault();");

                    stringEditor.NextThatContains($"{options.EntityNamePluralTo}CrudLogic {options.EntityNamePluralLowerTo}CrudLogic = new {options.EntityNamePluralTo}CrudLogic");
                    stringEditor.Next(line => !line.Contains("CrudRepository.Object"));
                    stringEditor.InsertLine($"                {options.EntityNamePluralLowerFrom}CrudRepository.Object,");
                }
                else
                {
                    stringEditor.NextThatContains($"{options.EntityNamePluralTo}CrudLogic {options.EntityNamePluralLowerTo}CrudLogic = new {options.EntityNamePluralTo}CrudLogic");
                    stringEditor.Next(line => !line.Contains("CrudRepository.Object"));
                    stringEditor.InsertLine("                null,");
                }
                stringEditor.NextThatContains("[TestMethod]");
            }

            return stringEditor.GetText();
        }
    }
}