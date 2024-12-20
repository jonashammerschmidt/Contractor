using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntityDtoForPurposeToMethodsAddition
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EntityDtoForPurposeToMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void AddRelationSideToBackendGeneratedFile(RelationSide relationSide, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackendGenerated(relationSide, paths);
            string fileData = fileSystemClient.ReadAllText(relationSide, filePath);
            fileData = UpdateFileData(relationSide, fileData);

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{relationSide.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains(": base(" + relationSide.Entity.NameLower);
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine($"            this.{relationSide.Name} = {relationSide.Entity.Name.LowerFirstChar()}.{relationSide.Name};");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains(": base(ef");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));
            stringEditor.InsertLine(
                $"            this.{relationSide.Name} = {relationSide.Type}\n" +
                $"                .FromEf{relationSide.OtherEntity.Name}Dto(ef{relationSide.Entity.Name}Dto.{relationSide.Name});");

            return stringEditor.GetText();
        }
    }
}