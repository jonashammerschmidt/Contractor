﻿using Contractor.Core.Helpers;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class DtoRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DtoRelationAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, namespacesToAdd);
        }

        public void AddRelationToDTOForDatabase(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, true, namespacesToAdd);
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, params string[] namespacesToAdd)
        {
            this.AddRelationToDTO(relationSide, domainFolder, templateFileName, forInterface, false, namespacesToAdd);
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, bool forDatabase, params string[] namespacesToAdd)
        {
            string filePath = (forDatabase) ?
                this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName) :
                this.pathService.GetAbsolutePathForBackend(relationSide, domainFolder, templateFileName);

            string fileData = UpdateFileData(relationSide, filePath, forInterface);

            if (namespacesToAdd != null)
            {
                foreach (string namespaceToAdd in namespacesToAdd)
                {
                    fileData = UsingStatements.Add(fileData, namespaceToAdd);
                }
            }

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(RelationSide relationSide, string filePath, bool forInterface)
        {
            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);

            fileData = AddUsingStatements(relationSide, fileData);
            fileData = AddProperty(fileData, relationSide, forInterface);

            return fileData;
        }

        private string AddUsingStatements(RelationSide relationSide, string fileData)
        {
            if (relationSide.Type == "Guid")
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            if (relationSide.Type.Contains("Enumerable") || relationSide.Type.Contains("ICollection"))
            {
                fileData = UsingStatements.Add(fileData, "System.Collections.Generic");
            }

            return fileData;
        }

        private string AddProperty(string file, RelationSide relationSide, bool forInterface)
        {
            StringEditor stringEditor = new StringEditor(file);
            FindStartingLineForNewProperty(file, relationSide, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (ContainsProperty(file))
            {
                stringEditor.InsertNewLine();
            }

            string optionalText = (relationSide.IsOptional && relationSide.Type == "Guid") ? "?" : "";
            if (forInterface)
                stringEditor.InsertLine($"        {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");
            else
                stringEditor.InsertLine($"        public {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");

            return stringEditor.GetText();
        }

        private void FindStartingLineForNewProperty(string file, RelationSide relationSide, StringEditor stringEditor)
        {
            bool hasConstructor = Regex.IsMatch(file, $"public .*{relationSide.Entity.Name}.*\\(");
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