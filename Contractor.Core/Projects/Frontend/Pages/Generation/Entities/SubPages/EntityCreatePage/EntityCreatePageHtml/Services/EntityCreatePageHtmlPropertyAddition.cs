using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageHtmlPropertyAddition
    {
        public PathService pathService;

        public EntityCreatePageHtmlPropertyAddition(
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
            stringEditor.InsertLine("        <br>");
            stringEditor.InsertNewLine();

            stringEditor.InsertLine(GetLine(options));

            return stringEditor.GetText();
        }

        private string GetLine(IPropertyAdditionOptions options)
        {
            if (options.PropertyType == "bool")
            {
                return 
                    $"        <mat-checkbox [(ngModel)]=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                    $"            {options.PropertyName.ToReadable()}\n" +
                    $"        </mat-checkbox>";
            }
            else if (options.PropertyType == "DateTime")
            {
                return
                   "        <mat-form-field appearance=\"outline\">\n" +
                  $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                  $"            <input matInput placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\" [matDatepicker]=\"picker\">\n" +
                   "            <mat-datepicker-toggle matSuffix [for]=\"picker\"></mat-datepicker-toggle>\n" +
                   "            <mat-datepicker #picker></mat-datepicker>\n" +
                   "        </mat-form-field>";
            }
            else if (options.PropertyType == "string" && string.IsNullOrEmpty(options.PropertyTypeExtra))
            {
                return
                  "        <mat-form-field appearance=\"outline\">\n" +
                 $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                 $"            <input matInput maxlength=\"{options.PropertyTypeExtra}\" placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                  "        </mat-form-field>";
            }
            else
            {
                return
                  "        <mat-form-field appearance=\"outline\">\n" +
                 $"            <mat-label>{options.PropertyName.ToReadable()}</mat-label>\n" +
                 $"            <input matInput placeholder=\"{options.PropertyName.ToReadable()}\" [(ngModel)]=\"{options.EntityName.LowerFirstChar()}.{options.PropertyName.LowerFirstChar()}\">\n" +
                  "        </mat-form-field>";
            }
        }
    }
}