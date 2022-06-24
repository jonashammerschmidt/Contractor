﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Logic
{
    internal class DbEntityMethodsAddition : PropertyAdditionToExisitingFileGeneration
    {
        public DbEntityMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("From" + property.Entity.Name + "Create(");
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = {property.Entity.NameLower}Create.{property.Name},");

            return stringEditor.GetText();
        }
    }
}