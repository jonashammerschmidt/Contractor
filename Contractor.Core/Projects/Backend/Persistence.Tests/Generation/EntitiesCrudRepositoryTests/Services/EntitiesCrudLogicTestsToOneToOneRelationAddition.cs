using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class EntitiesCrudRepositoryTestsToOneToOneRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesCrudRepositoryTestsToOneToOneRelationAddition(
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
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"public void Get{options.EntityNameTo}DefaultTest()");
            stringEditor.Prev();

            stringEditor.InsertLine("        [TestMethod]");
            stringEditor.InsertLine($"        public void Is{options.PropertyNameFrom}IdInUseTrueTest()");
            stringEditor.InsertLine("        {");
            stringEditor.InsertLine("            // Arrange");
            stringEditor.InsertLine($"            {options.EntityNamePluralTo}CrudRepository {options.EntityNamePluralLowerTo}CrudRepository = this.Get{options.EntityNamePluralTo}CrudRepositoryDefault();");
            stringEditor.InsertLine("");
            stringEditor.InsertLine("            // Act");
            stringEditor.InsertLine($"            bool is{options.PropertyNameFrom}IdInUse = {options.EntityNamePluralLowerTo}CrudRepository.Is{options.PropertyNameFrom}IdInUse({options.EntityNameTo}TestValues.{options.PropertyNameFrom}IdDbDefault);");
            stringEditor.InsertLine("");
            stringEditor.InsertLine("            // Assert");
            stringEditor.InsertLine($"            Assert.IsTrue(is{options.PropertyNameFrom}IdInUse);");
            stringEditor.InsertLine("        }");
            stringEditor.InsertLine("");
            stringEditor.InsertLine("        [TestMethod]");
            stringEditor.InsertLine($"        public void Is{options.PropertyNameFrom}IdInUseFalseTest()");
            stringEditor.InsertLine("        {");
            stringEditor.InsertLine("            // Arrange");
            stringEditor.InsertLine($"            {options.EntityNamePluralTo}CrudRepository {options.EntityNamePluralLowerTo}CrudRepository = this.Get{options.EntityNamePluralTo}CrudRepositoryDefault();");
            stringEditor.InsertLine("");
            stringEditor.InsertLine("            // Act");
            stringEditor.InsertLine($"            bool is{options.PropertyNameFrom}IdInUse = {options.EntityNamePluralLowerTo}CrudRepository.Is{options.PropertyNameFrom}IdInUse(Guid.NewGuid());");
            stringEditor.InsertLine("");
            stringEditor.InsertLine("            // Assert");
            stringEditor.InsertLine($"            Assert.IsFalse(is{options.PropertyNameFrom}IdInUse);");
            stringEditor.InsertLine("        }");
            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }
    }
}