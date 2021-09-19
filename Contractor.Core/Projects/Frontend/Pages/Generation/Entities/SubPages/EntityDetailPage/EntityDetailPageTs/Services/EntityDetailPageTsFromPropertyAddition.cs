using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageTsFromPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityDetailPageTsFromPropertyAddition(
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

            fileData = ImportStatements.Add(fileData, "MatTableDataSource", "@angular/material/table");
            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameTo}",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameTo)}");

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

            // ----------- DbSet -----------
            stringEditor.NextThatContains($"export class {options.EntityNameFrom}DetailPage");
            stringEditor.Next();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"  public {options.PropertyNameTo.LowerFirstChar()}TableDataSource = new MatTableDataSource<I{options.EntityNameTo}>([]);");
            stringEditor.InsertLine($"  public {options.PropertyNameTo.LowerFirstChar()}GridColumns: string[] = [");
            stringEditor.InsertLine($"    'bezeichnung',");
            stringEditor.InsertLine($"    'detail',");
            stringEditor.InsertLine($"  ];");

            stringEditor.NextThatContains($"private async load{options.EntityNameFrom}(");
            stringEditor.NextThatContains("  }");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.{options.PropertyNameTo.LowerFirstChar()}TableDataSource.data = this.{options.EntityNameLowerFrom}.{options.PropertyNameTo.LowerFirstChar()};");

            return stringEditor.GetText();
        }
    }
}