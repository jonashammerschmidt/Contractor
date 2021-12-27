using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntitiesCrudLogicTestsRelationAddition : RelationAdditionEditor
    {
        public EntitiesCrudLogicTestsRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Repository Generation -----------
            StringEditor stringEditor = new StringEditor(fileData);
            if (!fileData.Contains($"Setup{options.EntityNamePluralFrom}CrudRepositoryDefault()"))
            {
                stringEditor.MoveToEnd();
                stringEditor.Next();
                stringEditor.PrevThatContains("}");
                stringEditor.PrevThatContains("}");
                stringEditor.InsertLine("\n" +
                     $"        private Mock<I{options.EntityNamePluralFrom}CrudRepository> Setup{options.EntityNamePluralFrom}CrudRepositoryDefault()\n" +
                      "        {\n" +
                     $"            var {options.EntityNamePluralLowerFrom}CrudRepository = new Mock<I{options.EntityNamePluralFrom}CrudRepository>(MockBehavior.Strict);\n" +
                     $"            {options.EntityNamePluralLowerFrom}CrudRepository.Setup(repository => repository.Does{options.EntityNameFrom}Exist({options.EntityNameFrom}TestValues.IdDefault)).Returns(true);\n" +
                     $"            {options.EntityNamePluralLowerFrom}CrudRepository.Setup(repository => repository.Does{options.EntityNameFrom}Exist({options.EntityNameFrom}TestValues.IdDefault2)).Returns(true);\n" +
                     $"            {options.EntityNamePluralLowerFrom}CrudRepository.Setup(repository => repository.Does{options.EntityNameFrom}Exist({options.EntityNameFrom}TestValues.IdForCreate)).Returns(false);\n" +
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
                        stringEditor.InsertLine($"            Mock<I{options.EntityNamePluralFrom}CrudRepository> {options.EntityNamePluralLowerFrom}CrudRepository = this.Setup{options.EntityNamePluralFrom}CrudRepositoryDefault();");

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
            }

            return stringEditor.GetText();
        }
    }
}