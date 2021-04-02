﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class DtoRelationAddition
    {
        public PathService pathService;

        public DtoRelationAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddRelationToDTO(IRelationSideAdditionOptions options, string domainFolder, string templateFileName)
        {
            AddRelationToDTO(options, domainFolder, templateFileName, false);
        }

        public void AddRelationToDTO(IRelationSideAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            AddRelationToDTO(options, domainFolder, templateFileName, false, namespaceToAdd);
        }

        public void AddRelationToDTO(IRelationSideAdditionOptions options, string domainFolder, string templateFileName, bool forInterface)
        {
            AddRelationToDTO(options, domainFolder, templateFileName, forInterface, null);
        }

        public void AddRelationToDTO(IRelationSideAdditionOptions options, string domainFolder, string templateFileName, bool forInterface, string namespaceToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath, forInterface);

            if (namespaceToAdd != null)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IRelationSideAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationSideAdditionOptions options, string filePath, bool forInterface)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = AddUsingStatements(options, fileData);
            fileData = AddProperty(fileData, options, forInterface);

            return fileData;
        }

        private string AddUsingStatements(IRelationSideAdditionOptions options, string fileData)
        {
            if (options.PropertyType == "Guid")
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            if (options.PropertyType.Contains("Enumerable"))
            {
                fileData = UsingStatements.Add(fileData, "System.Collections.Generic");
            }

            return fileData;
        }

        private string AddProperty(string file, IRelationSideAdditionOptions options, bool forInterface)
        {
            StringEditor stringEditor = new StringEditor(file);
            FindStartingLineForNewProperty(file, options, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (ContainsProperty(file))
            {
                stringEditor.InsertNewLine();
            }

            if (forInterface)
                stringEditor.InsertLine($"        {options.PropertyType} {options.PropertyName} {{ get; set; }}");
            else
                stringEditor.InsertLine($"        public {options.PropertyType} {options.PropertyName} {{ get; set; }}");

            return stringEditor.GetText();
        }

        private void FindStartingLineForNewProperty(string file, IRelationSideAdditionOptions options, StringEditor stringEditor)
        {
            bool hasConstructor = Regex.IsMatch(file, $"public .*{options.EntityName}.*\\(");
            bool hasProperty = file.Contains("{ get; set; }");
            if (hasConstructor && hasProperty)
            {
                stringEditor.NextThatContains("{ get; set; }");
            }
            else
            {
                stringEditor.NextThatContains("{")
                          .NextThatContains("{");
            }
            stringEditor.Next(line => !IsLineEmpty(line) && !ContainsProperty(line));
        }

        private bool IsLineEmpty(string line)
        {
            return line.Trim().Length == 0;
        }

        private bool ContainsProperty(string line)
        {
            return line.Replace(" ", "").Contains("{get;set;}");
        }
    }
}