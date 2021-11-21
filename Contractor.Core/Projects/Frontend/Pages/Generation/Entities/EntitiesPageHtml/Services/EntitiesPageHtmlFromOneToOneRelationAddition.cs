using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageHtmlFromOneToOneRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesPageHtmlFromOneToOneRelationAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForFrom(options);
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontend(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(entityOptions.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("<table mat-table");
            stringEditor.NextThatContains("<ng-container matColumnDef=\"detail\">");
            stringEditor.InsertLine(GetAppRoutingLine(options));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(IRelationAdditionOptions options)
        {
            return
             $"            <ng-container matColumnDef=\"{options.PropertyNameTo.LowerFirstChar()}\">\n" +
             $"                <th mat-header-cell *matHeaderCellDef> {options.PropertyNameTo.ToReadable()} </th>\n" +
              "                <td mat-cell *matCellDef=\"let element\">\n" +
             $"                    <span *ngIf=\"element.{options.PropertyNameTo.LowerFirstChar()}\">{{{{element.{options.PropertyNameTo.LowerFirstChar()}.bezeichnung}}}}</span>\n" +
             $"                    <span *ngIf=\"!element.{options.PropertyNameTo.LowerFirstChar()}\">-</span>\n" +
              "                </td>\n" +
              "            </ng-container>\n";
        }
    }
}