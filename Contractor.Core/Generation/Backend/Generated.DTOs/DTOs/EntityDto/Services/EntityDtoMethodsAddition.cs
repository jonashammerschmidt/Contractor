using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class EntityDtoMethodsAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDtoMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("public " + property.Entity.Name + "Dto(" + property.Entity.Name + "Dto");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            this.{property.Name} = {property.Entity.Name.LowerFirstChar()}Dto.{property.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            if (property.Entity.IdType == "AutoIncrement")
            {
                stringEditor.NextThatContains("public " + property.Entity.Name + "Dto(int id");
            }
            else
            {
                stringEditor.NextThatContains("public " + property.Entity.Name + "Dto(Guid id");
            }
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            this.{property.Name} = {property.Entity.Name.LowerFirstChar()}DtoData.{property.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {property.Name} = ef{property.Entity.Name}.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("From" + property.Entity.Name + "DtoData");
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {property.Name} = {property.Entity.Name.LowerFirstChar()}DtoData.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("ToEf" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {property.Name} = {property.Entity.Name.LowerFirstChar()}Dto.{property.Name},");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}