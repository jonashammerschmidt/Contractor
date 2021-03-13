using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System;
using System.IO;

namespace Contractor.Core.Projects.Persistence.Tests
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
            stringEditor.InsertLine($"        public static readonly {options.PropertyType} {options.PropertyName}DbDefault = {GetValueForProperty(options, "DbDefault")};");
            stringEditor.InsertLine($"        public static readonly {options.PropertyType} {options.PropertyName}DbDefault2 = {GetValueForProperty(options, "DbDefault2")};");
            stringEditor.InsertLine($"        public static readonly {options.PropertyType} {options.PropertyName}ForCreate = {GetValueForProperty(options, "ForCreate")};");
            stringEditor.InsertLine($"        public static readonly {options.PropertyType} {options.PropertyName}ForUpdate = {GetValueForProperty(options, "ForUpdate")};");

            return stringEditor.GetText();
        }

        private string GetValueForProperty(IPropertyAdditionOptions options, string scope)
        {
            if (options.PropertyType == "string")
            {
                return "\"" + options.PropertyName + scope + "\"";
            }
            else if (options.PropertyType == "int")
            {
                return new Random().Next(100, 999).ToString();
            }
            else if (options.PropertyType == "Guid")
            {
                return $"Guid.Parse(\"{Guid.NewGuid()}\")";
            }
            else if (options.PropertyType == "bool")
            {
                return scope.Equals("DbDefault").ToString().ToLower();
            }
            else if (options.PropertyType == "DateTime")
            {
                Random gen = new Random();
                int range = 10 * 365; // 10 years          
                var randomDate = DateTime.Today.AddDays(-gen.Next(range));
                return $"new DateTime({randomDate.Year}, {randomDate.Month}, {randomDate.Day})";
            }

            return $"// TODO: {options.PropertyType} {options.PropertyName}";
        }
    }
}