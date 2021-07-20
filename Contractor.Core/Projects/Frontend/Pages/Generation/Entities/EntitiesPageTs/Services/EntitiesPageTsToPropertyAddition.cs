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

            fileData = ImportStatements.Add(fileData, "MultiDataSource",
                "src/app/components/ui/table-filter-bar/table-filter-bar-dropdown-multi/multi-data-source");

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}-crud.service");

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

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");
            stringEditor.InsertLine($"    '{options.PropertyNameFrom.LowerFirstChar()}',");

            stringEditor.NextThatContains("constructor(");
            stringEditor.Next();
            stringEditor.InsertLine($"    private {options.EntityNamePluralLowerFrom}CrudService: {options.EntityNamePluralFrom}CrudService,");

            stringEditor.NextThatContains("ngAfterViewInit()");
            int lineNumber = stringEditor.GetLineNumber();
            stringEditor.Next(line => !line.Trim().StartsWith("await this.setup"));
            stringEditor.InsertLine($"    await this.setup{options.EntityNamePluralFrom}Filter();");
            if (stringEditor.GetLineNumber() - lineNumber == 2)
            {
                stringEditor.InsertNewLine();
            }

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
                 "    this.filterItems.push({\n" +
                $"      dataName: '{options.PropertyNameFrom.ToReadable()}',\n" +
                 "      dataSource: new MultiDataSource((pageSize: number, pageIndex: number, filterTerm: string) => {\n" +
                $"        return this.{options.EntityNamePluralLowerFrom}CrudService.getPaged{options.EntityNamePluralFrom}({{\n" +
                 "          limit: pageSize,\n" +
                 "          offset: pageSize * pageIndex,\n" +
                 "          filters: [\n" +
                 "            {\n" +
                 "              filterField: 'name',\n" +
                 "              containsFilters: [filterTerm]\n" +
                 "            }\n" +
                 "          ]\n" +
                 "        });\n" +
                 "      }),\n" +
                 "      valueExpr: 'id',\n" +
                 "      displayExpr: 'name',\n" +
                 "    });\n" +
                 "\n" +
                 "    this.filterValues.push([]);\n" +
                 "  }";
        }
    }
}
