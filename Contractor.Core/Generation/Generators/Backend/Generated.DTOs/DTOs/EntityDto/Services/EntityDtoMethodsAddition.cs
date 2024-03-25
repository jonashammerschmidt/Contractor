using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntityDtoMethodsAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDtoMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("ToEf" + property.Entity.Name + "Dto");
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {property.Name} = {property.Entity.Name.LowerFirstChar()}Dto.{property.Name},");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}