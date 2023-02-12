using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

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
            stringEditor.NextThatContains("FromEf" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = ef{property.Entity.Name}.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("From" + property.Entity.Name + "DtoNoId");
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = {property.Entity.Name.LowerFirstChar()}.{property.Name},");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("ToEf" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = {property.Entity.Name.LowerFirstChar()}.{property.Name},");

            return stringEditor.GetText();
        }
    }
}