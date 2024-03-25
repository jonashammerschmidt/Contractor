using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntityDtoDataMethodsAddition : PropertyAdditionToExisitingFileGeneration
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
            stringEditor.NextThatContains("protected " + property.Entity.Name + "DtoData(Ef" + property.Entity.Name + "Dto");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            this.{property.Name} = ef{property.Entity.Name}Dto.{property.Name};");
            fileData = stringEditor.GetText();
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateEf" + property.Entity.Name + "Dto");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            ef{property.Entity.Name}Dto.{property.Name} = {property.Entity.Name.LowerFirstChar()}DtoData.{property.Name};");

            return stringEditor.GetText();
        }
    }
}