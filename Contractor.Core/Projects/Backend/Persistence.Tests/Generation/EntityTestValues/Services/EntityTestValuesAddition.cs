using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
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
            Random random = new Random(IntHash.ComputeIntHash($"{options.EntityName}.{options.PropertyName}"));

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.Next();
            stringEditor.PrevThatContains("}");
            stringEditor.PrevThatContains("}");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}DbDefault = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "DbDefault", random)};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}DbDefault2 = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "DbDefault2", random)};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}ForCreate = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "ForCreate", random)};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}ForUpdate = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "ForUpdate", random)};");

            return stringEditor.GetText();
        }
    }
}