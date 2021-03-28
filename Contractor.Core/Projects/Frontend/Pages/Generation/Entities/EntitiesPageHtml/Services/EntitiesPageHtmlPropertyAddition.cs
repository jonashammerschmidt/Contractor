using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlPropertyAddition
    {
        public PathService pathService;

        public EntitiesPageHtmlPropertyAddition(
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
            stringEditor.NextThatContains("<table mat-table");
            stringEditor.NextThatContains("<ng-container matColumnDef=\"detail\">");

            stringEditor.InsertLine(GetAppRoutingLine(options));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(IPropertyAdditionOptions options)
        {
            if (options.PropertyType == PropertyTypes.Boolean)
            {
                return
                  $"            <ng-container matColumnDef=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                  $"                <th mat-header-cell *matHeaderCellDef mat-sort-header> {options.PropertyName.ToReadable()} </th>\n" +
                  $"                <td mat-cell *matCellDef=\"let element\">\n" +
                  $"                    <mat-icon color=\"accent\" *ngIf=\"element.{options.PropertyName.LowerFirstChar()}\" >\n" +
                  $"                        check_box\n" +
                  $"                    </mat-icon>\n" +
                  $"                    <mat-icon style=\"color: gray\" *ngIf=\"!element.{options.PropertyName.LowerFirstChar()}\">\n" +
                  $"                        check_box_outline_blank\n" +
                  $"                    </mat-icon>\n" +
                  $"                </td>\n" +
                  $"            </ng-container>\n";
            }
            else if (options.PropertyType == PropertyTypes.DateTime)
            {
                return
                 $"            <ng-container matColumnDef=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                 $"                <th mat-header-cell *matHeaderCellDef mat-sort-header> {options.PropertyName.ToReadable()} </th>\n" +
                 $"                <td mat-cell *matCellDef=\"let element\"> {{{{element.{options.PropertyName.LowerFirstChar()} | date:'dd. MMM. yyyy, HH:mm'}}}} </td>\n" +
                  "            </ng-container>\n";
            }
            else
            {
                return
                 $"            <ng-container matColumnDef=\"{options.PropertyName.LowerFirstChar()}\">\n" +
                 $"                <th mat-header-cell *matHeaderCellDef mat-sort-header> {options.PropertyName.ToReadable()} </th>\n" +
                 $"                <td mat-cell *matCellDef=\"let element\" > {{{{element.{options.PropertyName.LowerFirstChar()}}}}} </td>\n" +
                  "            </ng-container>\n";
            }
        }
    }
}