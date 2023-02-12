using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class EntityDtoExpandedMethodsAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDtoExpandedMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("public " + property.Entity.Name + "DtoExpanded(" + property.Entity.Name + "DtoExpanded");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            this.{property.Name} = {property.Entity.Name.LowerFirstChar()}.{property.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + property.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {property.Name} = ef{property.Entity.Name}.{property.Name},");

            return stringEditor.GetText();
        }
    }
}