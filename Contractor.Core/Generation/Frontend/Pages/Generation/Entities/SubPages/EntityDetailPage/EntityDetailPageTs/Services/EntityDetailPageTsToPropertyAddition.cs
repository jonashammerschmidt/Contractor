using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityDetailPageTsToPropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityDetailPageTsToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{relationSide.OtherEntity.NamePlural}CrudService",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}-crud.service");

            fileData = ImportStatements.Add(fileData, $"I{relationSide.OtherEntity.Name}DtoExpanded",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(relationSide.OtherEntity.Name)}-dto-expanded");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"export interface {relationSide.Entity.Name}DetailPageForm");
            stringEditor.NextThatContains("}");
            stringEditor.InsertLine($"  {relationSide.NameLower}Id{(relationSide.IsOptional ? "?" : "" )}: string;");

            stringEditor.NextThatContains("constructor(");
            stringEditor.InsertLine($"  {relationSide.NameLower}DataSource: DropdownDataSource<I{relationSide.OtherEntity.Name}DtoExpanded>;");
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("private formBuilder: UntypedFormBuilder");

            string constructorLine = $"    private {relationSide.OtherEntity.NamePluralLower}CrudService: {relationSide.OtherEntity.NamePlural}CrudService,";
            if (!fileData.Contains(constructorLine))
            {
                stringEditor.InsertLine(constructorLine);
            }

            stringEditor.NextThatContains("initializeDataSources() {");
            stringEditor.NextThatStartsWith("  }");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.{relationSide.NameLower}DataSource = new DropdownDataSource({{");
            stringEditor.InsertLine($"      filterField: '{relationSide.OtherEntity.DisplayProperty.NameLower}',");
            stringEditor.InsertLine($"      getPagedData: (options) => this.{relationSide.OtherEntity.NamePluralLower}CrudService.getPaged{relationSide.OtherEntity.NamePlural}(options),");
            stringEditor.InsertLine($"    }});");
            
            stringEditor.NextThatContains("setupFormController() {");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"      {relationSide.NameLower}Id: {{");
            if (!relationSide.IsOptional)
            {
                stringEditor.InsertLine($"        validators: [Validators.required],");
            }
            stringEditor.InsertLine($"      }},");

            return stringEditor.GetText();
        }
    }
}