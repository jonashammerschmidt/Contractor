﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlToPropertyAddition
    {
        public PathService pathService;

        public EntityDetailPageHtmlToPropertyAddition(
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

            if (fileData.Contains("</mat-tab-group>"))
            {
                stringEditor.NextThatContains("<mat-tab label=\"Stammdaten\">");
                stringEditor.NextThatContains("</mat-tab>");
            }
            else
            {
                stringEditor.NextThatStartsWith($"<div class=\"{options.EntityNameTo.ToKebab()}-detail-page\"");
                stringEditor.NextThatStartsWith($"</div>");
            }

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return
                $"    <p>\n" +
                $"        <span style=\"font-size: 0.8em;\">{options.PropertyNameFrom.ToReadable()}:</span>\n" +
                $"        <br>\n" +
                $"        <a [routerLink]=\"['/{StringConverter.PascalToKebabCase(options.DomainFrom)}/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}/detail', {options.EntityNameLowerTo}.{options.PropertyNameFrom.LowerFirstChar()}.id]\">\n" +
                $"            {{{{{options.EntityNameLowerTo}.{options.PropertyNameFrom.LowerFirstChar()}.name}}}}\n" +
                $"            <mat-icon style=\"font-size: 1em;\">open_in_new</mat-icon>\n" +
                $"        </a>\n" +
                $"    </p>\n";
        }
    }
}