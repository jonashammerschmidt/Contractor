using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageTsToPropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityCreatePageTsToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{relationSide.OtherEntity.NamePlural}CrudService",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}-crud.service");

            fileData = ImportStatements.Add(fileData, $"I{relationSide.OtherEntity.Name}ListItem",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(relationSide.OtherEntity.Name)}-list-item");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("constructor(");
            stringEditor.InsertLine($"  {relationSide.NameLower}DataSource: DropdownPaginationDataSource<I{relationSide.OtherEntity.Name}ListItem>;");
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("private formBuilder: FormBuilder");

            string constructorLine = $"    private {relationSide.OtherEntity.NamePluralLower}CrudService: {relationSide.OtherEntity.NamePlural}CrudService,";
            if (!fileData.Contains(constructorLine))
            {
                stringEditor.InsertLine(constructorLine);
            }
            stringEditor.NextThatContains("this.formBuilder.group({");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"      {relationSide.NameLower}Id: new FormControl(null, [" +
                ((!relationSide.IsOptional) ? "Validators.required" : "") +
                "]),");

            stringEditor.MoveToStart();
            stringEditor.NextThatContains("ngOnInit()");
            stringEditor.NextThatStartsWith("  }");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.{relationSide.NameLower}DataSource = new DropdownPaginationDataSource(");
            stringEditor.InsertLine($"      (options) => this.{relationSide.OtherEntity.NamePluralLower}CrudService.getPaged{relationSide.OtherEntity.NamePlural}(options),");
            stringEditor.InsertLine($"      '{relationSide.OtherEntity.DisplayProperty.NameLower}');");

            return stringEditor.GetText();
        }
    }
}