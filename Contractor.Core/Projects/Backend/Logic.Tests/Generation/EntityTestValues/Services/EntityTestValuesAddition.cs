using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityTestValuesAddition
    {
        public PathService pathService;

        public EntityTestValuesAddition(PathService pathService)
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
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.Next();
            stringEditor.PrevThatContains("}");
            stringEditor.PrevThatContains("}");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}Default = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "Default")};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}ForCreate = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "ForCreate")};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}ForUpdate = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "ForUpdate")};");

            return stringEditor.GetText();
        }
    }
}