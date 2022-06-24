﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityListItemFromOneToOneMethodsAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public DbEntityListItemFromOneToOneMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{relationSide.Entity.Module.Options.Paths.ProjectName}.Persistence.Modules.{relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + relationSide.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine(
                $"                {relationSide.Name} = Db{relationSide.OtherEntity.Name}\n" +
                $"                    .FromEf{relationSide.OtherEntity.Name}(ef{relationSide.Entity.Name}.{relationSide.Name}),");

            return stringEditor.GetText();
        }
    }
}