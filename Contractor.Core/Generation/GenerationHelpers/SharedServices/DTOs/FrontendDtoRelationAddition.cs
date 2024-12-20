﻿using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    public class FrontendDtoRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendDtoRelationAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(RelationSide relationSide, string domainFolder, string templateFileName, string importStatementTypes, string importStatementPath)
        {
            string filePath = this.pathService.GetAbsolutePathForFrontend(relationSide, domainFolder, templateFileName);
            string fileData = UpdateFileData(relationSide, filePath);

            fileData = ImportStatements.Add(fileData, importStatementTypes, importStatementPath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(RelationSide relationSide, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            if (!stringEditor.GetLine().Contains("export interface"))
            {
                stringEditor.NextThatStartsWith($"export interface");
            }
            stringEditor.NextThatContains("}");

            string optionalText = (relationSide.IsOptional) ? "?" : "";
            stringEditor.InsertLine($"    {relationSide.NameLower}{optionalText}: {relationSide.Type};");

            return stringEditor.GetText();
        }
    }
}