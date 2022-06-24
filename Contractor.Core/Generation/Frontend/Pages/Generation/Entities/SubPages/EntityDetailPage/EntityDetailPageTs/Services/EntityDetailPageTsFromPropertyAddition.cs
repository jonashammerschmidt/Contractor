﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityDetailPageTsFromPropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityDetailPageTsFromPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "MatTableDataSource", "@angular/material/table");
            fileData = ImportStatements.Add(fileData, $"I{relationSide.OtherEntity.Name}",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/dtos/i-{relationSide.OtherEntity.NameKebab}");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"export class {relationSide.Entity.Name}DetailPage");
            stringEditor.Next();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"  public {relationSide.NameLower}TableDataSource = new MatTableDataSource<I{relationSide.OtherEntity.Name}>([]);");
            stringEditor.InsertLine($"  public {relationSide.NameLower}GridColumns: string[] = [");
            stringEditor.InsertLine($"    '{relationSide.OtherEntity.DisplayProperty.NameLower}',");
            stringEditor.InsertLine($"    'detail',");
            stringEditor.InsertLine($"  ];");

            stringEditor.NextThatContains($"private async load{relationSide.Entity.Name}(");
            stringEditor.NextThatContains("  }");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"    this.{relationSide.NameLower}TableDataSource.data = this.{relationSide.Entity.NameLower}.{relationSide.NameLower};");

            return stringEditor.GetText();
        }
    }
}