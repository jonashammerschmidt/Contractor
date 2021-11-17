using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageHtmlFromPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityDetailPageHtmlFromPropertyAddition(
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
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(entityOptions, domainFolder);
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

            if (!fileData.Contains("</mat-tab-group>"))
            {
                AddTabs(stringEditor);
            }

            AddTab(options, stringEditor);

            return stringEditor.GetText();
        }

        private void AddTabs(StringEditor stringEditor)
        {
            stringEditor.NextThatStartsWith("    <div class=\"toolbar\">");
            stringEditor.NextThatStartsWith("    </div>");
            stringEditor.Next();

            stringEditor.InsertLine("    <mat-tab-group mat-align-tabs=\"start\">");
            stringEditor.InsertLine("        <mat-tab label=\"Stammdaten\">");

            stringEditor.NextThatStartsWith("</div>");

            stringEditor.InsertLine("        </mat-tab>");
            stringEditor.InsertLine("    </mat-tab-group>");

            stringEditor.AddPrefixBetweenLinesThatContain("        ", "<mat-tab label=\"Stammdaten\">", " </mat-tab>");
        }

        private void AddTab(IRelationAdditionOptions options, StringEditor stringEditor)
        {
            stringEditor.MoveToStart();
            stringEditor.NextThatContains("    </mat-tab-group>");

            stringEditor.InsertLine($"        <mat-tab label=\"{options.PropertyNameTo.ToReadable()}\">");
            stringEditor.InsertLine($"            <h2>{options.PropertyNameTo.ToReadable()}</h2>");
            stringEditor.InsertLine($"            <div class=\"table-container\">");
            stringEditor.InsertLine($"                <table mat-table [dataSource]=\"{options.PropertyNameTo.LowerFirstChar()}TableDataSource\">");
            stringEditor.InsertLine($"");
            stringEditor.InsertLine($"                    <ng-container matColumnDef=\"bezeichnung\">");
            stringEditor.InsertLine($"                        <th mat-header-cell *matHeaderCellDef> Bezeichnung </th>");
            stringEditor.InsertLine($"                        <td mat-cell *matCellDef=\"let element\">");
            stringEditor.InsertLine($"                            {{{{element.bezeichnung}}}}");
            stringEditor.InsertLine($"                        </td>");
            stringEditor.InsertLine($"                    </ng-container>");
            stringEditor.InsertLine($"");
            stringEditor.InsertLine($"                    <ng-container matColumnDef=\"detail\">");
            stringEditor.InsertLine($"                        <th mat-header-cell *matHeaderCellDef></th>");
            stringEditor.InsertLine($"                        <td mat-cell *matCellDef=\"let element\" width=\"10%\">");
            stringEditor.InsertLine($"                            <button mat-button role=\"link\">Detail</button>");
            stringEditor.InsertLine($"                        </td>");
            stringEditor.InsertLine($"                    </ng-container>");
            stringEditor.InsertLine($"");
            stringEditor.InsertLine($"                    <tr mat-header-row *matHeaderRowDef=\"{options.PropertyNameTo.LowerFirstChar()}GridColumns; sticky: true\"></tr>");
            stringEditor.InsertLine($"                    <tr mat-row *matRowDef=\"let row; columns: {options.PropertyNameTo.LowerFirstChar()}GridColumns;\"");
            stringEditor.InsertLine($"                        [routerLink]=\"['/{StringConverter.PascalToKebabCase(options.DomainTo)}/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}/detail', row.id]\"></tr>");
            stringEditor.InsertLine($"                </table>");
            stringEditor.InsertLine($"            </div>");
            stringEditor.InsertLine($"        </mat-tab>");
        }
    }
}