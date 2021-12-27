using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class EntitiesCrudRepositoryTestsToOneToOneRelationAddition : RelationAdditionEditor
    {
        public EntitiesCrudRepositoryTestsToOneToOneRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
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