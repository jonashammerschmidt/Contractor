using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class EntityDtoDataMethodsAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDtoDataMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("public " + property.Entity.Name + "DtoData(" + property.Entity.Name + "DtoData");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            this.{property.Name} = {property.Entity.Name.LowerFirstChar()}DtoData.{property.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("To" + property.Entity.Name + "Dto");
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {property.Name} = {property.Entity.Name.LowerFirstChar()}DtoData.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateEf" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            ef{property.Entity.Name}.{property.Name} = {property.Entity.Name.LowerFirstChar()}DtoData.{property.Name};");

            return stringEditor.GetText();
        }
    }
}