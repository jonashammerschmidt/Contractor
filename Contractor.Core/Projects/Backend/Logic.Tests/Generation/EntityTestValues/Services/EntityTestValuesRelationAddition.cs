﻿
using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class EntityTestValuesRelationAddition
    {
        public PathService pathService;

        public EntityTestValuesRelationAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            var relationAdditionOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(relationAdditionOptions, domainFolder);
            string fileName = templateFileName.Replace("Entity", relationAdditionOptions.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Logic.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.Next();
            stringEditor.PrevThatContains("}");
            stringEditor.PrevThatContains("}");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdDefault = {options.EntityNameFrom}TestValues.IdDefault;");
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdDefault2 = {options.EntityNameFrom}TestValues.IdDefault2;");
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdForCreate = {options.EntityNameFrom}TestValues.IdDefault;");
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdForUpdate = {options.EntityNameFrom}TestValues.IdDefault2;");

            return stringEditor.GetText();
        }
    }
}