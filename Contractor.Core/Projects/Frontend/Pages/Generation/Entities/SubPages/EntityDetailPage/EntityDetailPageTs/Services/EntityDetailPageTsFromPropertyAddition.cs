using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityDetailPageTsFromPropertyAddition : RelationAdditionEditor
    {
        public EntityDetailPageTsFromPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "MatTableDataSource", "@angular/material/table");
            fileData = ImportStatements.Add(fileData, $"I{options.EntityNameTo}",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}" +
                $"/dtos/i-{StringConverter.PascalToKebabCase(options.EntityNameTo)}");

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