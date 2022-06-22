﻿using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailFromMethodsAddition : RelationAdditionEditor
    {
        public EntityDetailFromMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "System.Linq");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromDb" + relationSide.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine(
                $"                {relationSide.Name} = db{relationSide.Entity.Name}Detail.{relationSide.Name}\n" +
                $"                    .Select(db{relationSide.OtherEntity.Name} => {relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}.{relationSide.OtherEntity.Name}\n" +
                $"                        .FromDb{relationSide.OtherEntity.Name}(db{relationSide.OtherEntity.Name})),");

            return stringEditor.GetText();
        }
    }
}