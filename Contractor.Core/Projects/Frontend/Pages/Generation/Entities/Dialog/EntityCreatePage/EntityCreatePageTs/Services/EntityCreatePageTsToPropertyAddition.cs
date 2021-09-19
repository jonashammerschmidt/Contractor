using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageTsToPropertyAddition
    {
        public FileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityCreatePageTsToPropertyAddition(
            FileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}-crud.service");

            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameFrom}ListItem",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}-list-item");

            this.fileSystemClient.WriteAllText(filePath, fileData);
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
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("constructor(");
            stringEditor.InsertLine($"  {options.PropertyNameFrom.LowerFirstChar()}DataSource: DropdownPaginationDataSource<I{options.EntityNameFrom}ListItem>;");
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("private formBuilder: FormBuilder");

            string constructorLine = $"    private {options.EntityNamePluralLowerFrom}CrudService: {options.EntityNamePluralFrom}CrudService,";
            if (!fileData.Contains(constructorLine))
            {
                stringEditor.InsertLine(constructorLine);
            }
            stringEditor.NextThatContains("this.formBuilder.group({");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"      {options.PropertyNameFrom.LowerFirstChar()}Id: new FormControl(null, [Validators.required]),");

            stringEditor.MoveToStart();
            stringEditor.NextThatContains("ngOnInit()");
            stringEditor.NextThatStartsWith("  }");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.{options.PropertyNameFrom.LowerFirstChar()}DataSource = new DropdownPaginationDataSource(");
            stringEditor.InsertLine($"      (options) => this.{options.EntityNamePluralLowerFrom}CrudService.getPaged{options.EntityNamePluralFrom}(options),");
            stringEditor.InsertLine( "      'bezeichnung');");

            return stringEditor.GetText();
        }
    }
}