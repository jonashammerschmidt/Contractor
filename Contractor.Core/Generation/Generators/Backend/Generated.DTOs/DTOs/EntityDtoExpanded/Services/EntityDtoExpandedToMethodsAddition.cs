using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class EntityDtoExpandedToMethodsAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityDtoExpandedToMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{relationSide.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("public " + relationSide.Entity.Name + "DtoExpanded(" + relationSide.Entity.Name + "DtoExpanded");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            this.{relationSide.Name} = {relationSide.Entity.Name.LowerFirstChar()}.{relationSide.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("protected " + relationSide.Entity.Name + "DtoExpanded(Ef" + relationSide.Entity.Name + "Dto");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine(
                $"            this.{relationSide.Name} = {relationSide.OtherEntity.Name}Dto\n" +
                $"                .FromEf{relationSide.OtherEntity.Name}Dto(ef{relationSide.Entity.Name}Dto.{relationSide.Name});");

            return stringEditor.GetText();
        }
    }
}