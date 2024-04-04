using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class IEntitiesCrudRepositoryPurposeDtoInserter
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public IEntitiesCrudRepositoryPurposeDtoInserter(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Insert(PurposeDto purposeDto, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackend(purposeDto.Entity, paths);
            string fileData = fileSystemClient.ReadAllText(purposeDto.Entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.PrevThatContains("}");
            }

            stringEditor.PrevThatContains("}");
            stringEditor.InsertNewLine();
            
            var methodsToInsert = GetStringTemplate();
            methodsToInsert = methodsToInsert.Replace("Entities", purposeDto.Entity.NamePlural);
            methodsToInsert = methodsToInsert.Replace("Entity", purposeDto.Entity.Name);
            methodsToInsert = methodsToInsert.Replace("entities", purposeDto.Entity.NamePluralLower);
            methodsToInsert = methodsToInsert.Replace("entity", purposeDto.Entity.NameLower);
            methodsToInsert = methodsToInsert.Replace("Purpose", purposeDto.Purpose);
            stringEditor.InsertLine(methodsToInsert);

            fileSystemClient.WriteAllText(stringEditor.GetText(), filePath);
        }

        public string GetStringTemplate()
        {
            return
                "        EntityDtoForPurpose GetEntityForPurpose(Guid entityId);\n" +
                "\n" +
                "        IEnumerable<EntityDtoForPurpose> GetEntitiesForPurpose();\n" +
                "\n" +
                "        IDbPagedResult<EntityDtoForPurpose> GetPagedEntitiesForPurpose();";
        }
    }
}