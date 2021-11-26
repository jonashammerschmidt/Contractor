using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityTestMethodsAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbEntityTestMethodsAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForBackend(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            // ----------- Creators -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName} Default()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}Default,");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName} Default2()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}Default2,");
            fileData = stringEditor.GetText();

            // ----------- Asserts -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}Default, db{options.EntityName}.{options.PropertyName});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDefault2");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}Default2, db{options.EntityName}.{options.PropertyName});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertCreated");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}ForCreate, db{options.EntityName}.{options.PropertyName});");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}