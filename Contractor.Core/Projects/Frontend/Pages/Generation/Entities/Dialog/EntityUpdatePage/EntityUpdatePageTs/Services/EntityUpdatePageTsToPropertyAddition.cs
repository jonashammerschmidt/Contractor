using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageTsToPropertyAddition
    {
        public PathService pathService;

        public EntityUpdatePageTsToPropertyAddition(
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

            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameFrom}ListItem",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}-list-item");

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
            stringEditor.InsertLine($"  {options.EntityNamePluralLowerFrom}DataSource: MultiDataSource<I{options.EntityNameFrom}ListItem>;");
            stringEditor.InsertLine($"  selected{options.PropertyNameFrom}: I{options.EntityNameFrom}ListItem;");
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("private formBuilder: FormBuilder");
            stringEditor.InsertLine($"    private {options.EntityNamePluralLowerFrom}CrudService: {options.EntityNamePluralFrom}CrudService,");

            stringEditor.NextThatContains("this.formBuilder.group({");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"      {options.PropertyNameFrom.LowerFirstChar()}Id: new FormControl(null, [Validators.required]),");

            stringEditor.MoveToStart();
            stringEditor.NextThatContains("ngOnInit()");
            stringEditor.NextThatStartsWith("  }");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.selected{options.PropertyNameFrom} = {options.EntityNameLowerTo}Detail.{options.PropertyNameFrom.LowerFirstChar()};");
            stringEditor.InsertLine($"    this.{options.EntityNamePluralLowerFrom}DataSource = new MultiDataSource(");
            stringEditor.InsertLine("    (pageSize: number, pageIndex: number, filterTerm: string) => {");
            stringEditor.InsertLine($"        return this.{options.EntityNamePluralLowerFrom}CrudService.getPaged{options.EntityNamePluralFrom}({{");
            stringEditor.InsertLine("        limit: pageSize,");
            stringEditor.InsertLine("        offset: pageSize * pageIndex,");
            stringEditor.InsertLine("        filters: [");
            stringEditor.InsertLine("            {");
            stringEditor.InsertLine("            filterField: 'name',");
            stringEditor.InsertLine("            containsFilters: [filterTerm]");
            stringEditor.InsertLine("            }");
            stringEditor.InsertLine("        ]");
            stringEditor.InsertLine("        });");
            stringEditor.InsertLine("    });");

            return stringEditor.GetText();
        }
    }
}