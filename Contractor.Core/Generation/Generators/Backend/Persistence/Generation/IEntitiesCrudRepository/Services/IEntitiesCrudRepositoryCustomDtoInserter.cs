using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class IEntitiesCrudRepositoryCustomDtoInserter
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public IEntitiesCrudRepositoryCustomDtoInserter(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Insert(CustomDto customDto, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackend(customDto.Entity, paths);
            string fileData = fileSystemClient.ReadAllText(customDto.Entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.PrevThatContains("}");
            }

            stringEditor.PrevThatContains("}");
            stringEditor.InsertNewLine();
            
            var methodsToInsert = GetStringTemplate();
            methodsToInsert = methodsToInsert.Replace("Entities", customDto.Entity.NamePlural);
            methodsToInsert = methodsToInsert.Replace("Entity", customDto.Entity.Name);
            methodsToInsert = methodsToInsert.Replace("entities", customDto.Entity.NamePluralLower);
            methodsToInsert = methodsToInsert.Replace("entity", customDto.Entity.NameLower);
            methodsToInsert = methodsToInsert.Replace("Purpose", customDto.Purpose);
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