using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlPropertyAddition
    {
        public PathService pathService;

        public EntityDetailPageHtmlPropertyAddition(
            PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("</mat-card>");

            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IPropertyAdditionOptions options)
        {
            if (options.PropertyType == "bool")
            {
                return
                  $"        <p [attr.aria-label]=\"'{options.PropertyName.ToReadable()}: ' + ({options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}) ? 'aktiv' : 'inaktiv' \">\n" +
                  $"            <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{options.PropertyName.ToReadable()}:</span>\n" +
                  $"            <br>\n" +
                  $"            <mat-icon color=\"accent\" *ngIf=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                  $"                check_box\n" +
                  $"            </mat-icon>\n" +
                  $"            <mat-icon style=\"color: gray\" *ngIf=\"!{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                  $"                check_box_outline_blank\n" +
                  $"            </mat-icon>\n" +
                   "        </p>";
            }
            else if (options.PropertyType == "DateTime")
            {
                return
                     $"        <p [attr.aria-label]=\"'{options.PropertyName.ToReadable()}: ' + {options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                     $"            <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{options.PropertyName.ToReadable()}:</span>\n" +
                     $"            <br>\n" +
                     $"            <span aria-hidden=\"true\">{{{{{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()} | date:'dd. MMM. yyyy, HH:mm'}}}}</span>\n" +
                      "        </p>";
            }
            else
            {
                return
                     $"        <p [attr.aria-label]=\"'{options.PropertyName.ToReadable()}: ' + {options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                     $"            <span style=\"font-size: 0.8em;\" aria-hidden=\"true\">{options.PropertyName.ToReadable()}:</span>\n" +
                     $"            <br>\n" +
                     $"            <span aria-hidden=\"true\">{{{{{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}}}}}</span>\n" +
                      "        </p>";
            }
        }
    }
}