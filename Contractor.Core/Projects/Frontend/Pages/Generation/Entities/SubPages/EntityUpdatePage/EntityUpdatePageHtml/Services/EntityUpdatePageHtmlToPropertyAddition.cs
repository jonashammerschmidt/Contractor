﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlToPropertyAddition
    {
        public PathService pathService;

        public EntityUpdatePageHtmlToPropertyAddition(
            PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(entityOptions.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("</mat-card>");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine("        <br>");
            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return
              $"        <app-search-dropdown label=\"{options.EntityNameFrom}\" valueExpr=\"id\" [dataSource]=\"{options.EntityNamePluralLowerFrom}\" [(value)]=\"{options.EntityNameLowerTo}Update.{options.EntityNameLowerFrom}Id\"\n" +
               "            displayExpr=\"name\"></app-search-dropdown>";
        }
    }
}