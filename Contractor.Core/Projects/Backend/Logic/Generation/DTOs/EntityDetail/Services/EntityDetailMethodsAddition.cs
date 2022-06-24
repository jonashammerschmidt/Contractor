﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailMethodsAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDetailMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromDb" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = db{property.Entity.Name}Detail.{property.Name},");

            return stringEditor.GetText();
        }
    }
}