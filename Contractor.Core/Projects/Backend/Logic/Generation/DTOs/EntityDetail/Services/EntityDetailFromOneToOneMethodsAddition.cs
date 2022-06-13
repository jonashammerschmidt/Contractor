﻿using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailFromOneToOneMethodsAddition : RelationAdditionEditor
    {
        public EntityDetailFromOneToOneMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromDb" + relationSide.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine(
                $"                {relationSide.Name} = {relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}.{relationSide.OtherEntity.Name}\n" +
                $"                    .FromDb{relationSide.OtherEntity.Name}(db{relationSide.Entity.Name}Detail.{relationSide.Name}),");

            return stringEditor.GetText();
        }
    }
}