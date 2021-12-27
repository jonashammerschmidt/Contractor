using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageTsToPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntityUpdatePageTsToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}-crud.service");

            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameFrom}",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}");

            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameFrom}ListItem",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameFrom)}-list-item");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("constructor(");
            stringEditor.InsertLine($"  {options.PropertyNameFrom.LowerFirstChar()}DataSource: DropdownPaginationDataSource<I{options.EntityNameFrom}ListItem>;");
            stringEditor.InsertLine($"  selected{options.PropertyNameFrom}: I{options.EntityNameFrom};");
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("private formBuilder: FormBuilder");

            string constructorLine = $"    private {options.EntityNamePluralLowerFrom}CrudService: {options.EntityNamePluralFrom}CrudService,";
            if (!fileData.Contains(constructorLine))
            {
                stringEditor.InsertLine(constructorLine);
            }

            stringEditor.NextThatContains("this.formBuilder.group({");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"      {options.PropertyNameFrom.LowerFirstChar()}Id: new FormControl(null, [" +
                ((!options.IsOptional) ? "Validators.required" : "") +
                "]),");

            stringEditor.MoveToStart();
            stringEditor.NextThatContains("ngOnInit()");
            stringEditor.NextThatStartsWith("  }");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.selected{options.PropertyNameFrom} = {options.EntityNameLowerTo}Detail.{options.PropertyNameFrom.LowerFirstChar()};");
            stringEditor.InsertLine($"    this.{options.PropertyNameFrom.LowerFirstChar()}DataSource = new DropdownPaginationDataSource(");
            stringEditor.InsertLine($"      (options) => this.{options.EntityNamePluralLowerFrom}CrudService.getPaged{options.EntityNamePluralFrom}(options),");
            stringEditor.InsertLine( "      'bezeichnung');");

            return stringEditor.GetText();
        }
    }
}