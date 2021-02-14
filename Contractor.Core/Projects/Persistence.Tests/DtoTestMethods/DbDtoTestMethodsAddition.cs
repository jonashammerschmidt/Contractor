using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class DbDtoTestMethodsAddition
    {
        public PathService pathService;

        public DbDtoTestMethodsAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName} DatabaseDefault()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}DbDefault,");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName} ForCreate()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}ForCreate,");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName} ForUpdate()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}ForUpdate,");
            fileData = stringEditor.GetText();

            // ----------- Asserts -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertDbDefault");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}DbDefault, db{options.EntityName}.{options.PropertyName});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertForCreate");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}ForCreate, db{options.EntityName}.{options.PropertyName});");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("AssertForUpdate");
            stringEditor.Next(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            Assert.AreEqual({options.EntityName}TestValues.{options.PropertyName}ForUpdate, db{options.EntityName}.{options.PropertyName});");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}