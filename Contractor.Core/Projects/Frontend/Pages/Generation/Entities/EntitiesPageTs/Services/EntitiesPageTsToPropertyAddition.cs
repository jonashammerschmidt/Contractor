using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsToPropertyAddition
    {
        public PathService pathService;

        public EntitiesPageTsToPropertyAddition(
            PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}-crud.service");

            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameFrom}",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}");

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

            stringEditor.NextThatContains("constructor(");
            stringEditor.InsertLine($"  {options.EntityNamePluralLowerFrom}: I{options.EntityNameFrom}[];");
            stringEditor.InsertNewLine();

            stringEditor.Next();
            stringEditor.InsertLine($"    private {options.EntityNamePluralLowerFrom}CrudService: {options.EntityNamePluralFrom}CrudService,");

            stringEditor.NextThatContains("ngOnInit()");
            stringEditor.Next();
            stringEditor.InsertLine($"    await this.setup{options.EntityNamePluralFrom}Filter();");
            stringEditor.InsertNewLine();

            stringEditor.MoveToEnd();
            stringEditor.PrevThatContains("}");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine(GetSetupMethod(options));

            return stringEditor.GetText();
        }

        private string GetSetupMethod(IRelationAdditionOptions options)
        {
            return
                $"  private async setup{options.EntityNamePluralFrom}Filter(): Promise<void> {{\n" +
                $"    this.{options.EntityNamePluralLowerFrom} = await this.{options.EntityNamePluralLowerFrom}CrudService.get{options.EntityNamePluralFrom}();\n" +
                 "\n" +
                 "    this.filterItems.push({\n" +
                $"      dataName: '{options.EntityNamePluralFrom}',\n" +
                $"      dataSource: this.{options.EntityNamePluralLowerFrom},\n" +
                 "      valueExpr: 'id',\n" +
                 "      displayExpr: 'name',\n" +
                 "    });\n" +
                 "\n" +
                 "    const filterValuesIndex = this.filterValues.length;\n" +
                 "    this.filterValues.push([]);\n" +
                 "\n" +
                $"    this.filterComparators.push(({options.EntityNameLowerTo}) => {{\n" +
                 "      return this.filterValues[filterValuesIndex].length < 1 ||\n" +
                $"        this.filterValues[filterValuesIndex].includes({options.EntityNameLowerTo}.{options.EntityNameLowerFrom}Id);\n" +
                 "    });\n" +
                 "  }";
        }
    }
}