using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntitiesCrudRepositoryPurposeDtoInserter
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EntitiesCrudRepositoryPurposeDtoInserter(IFileSystemClient fileSystemClient, PathService pathService)
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
                "        public EntityDtoForPurpose GetEntityForPurpose(Guid entityId)\n" +
                "        {\n" +
                "            IQueryable<EfEntityDto> efEntities = this.dbContext.Entities;\n" +
                "            efEntities = EntityDtoForPurpose.AddIncludesToQuery(efEntities);\n" +
                "\n" +
                "            EfEntityDto efEntity = efEntities\n" +
                "                .Where(efEntityDto => efEntityDto.Id == entityId)\n" +
                "                .SingleOrDefault();\n" +
                "\n" +
                "            if (efEntity == null)\n" +
                "            {\n" +
                "                throw new NotFoundResultException(\"Entity ({id}) konnte nicht gefunden werden.\", entityId);\n" +
                "            }\n" +
                "\n" +
                "            return EntityDtoForPurpose.FromEfEntityDto(efEntity);\n" +
                "        }\n" +
                "\n" +
                "        public IEnumerable<EntityDtoForPurpose> GetEntitiesForPurpose()\n" +
                "        {\n" +
                "            IQueryable<EfEntityDto> efEntities = this.dbContext.Entities;\n" +
                "            efEntities = EntityDtoForPurpose.AddIncludesToQuery(efEntities);\n" +
                "\n" +
                "            return efEntities\n" +
                "                .Select(efEntity => EntityDtoForPurpose.FromEfEntityDto(efEntity))\n" +
                "                .ToList();\n" +
                "        }\n" +
                "\n" +
                "        public IDbPagedResult<EntityDtoForPurpose> GetPagedEntitiesForPurpose()\n" +
                "        {\n" +
                "            IQueryable<EfEntityDto> efEntities = this.dbContext.Entities;\n" +
                "            efEntities = EntityDtoForPurpose.AddIncludesToQuery(efEntities);\n" +
                "\n" +
                "            return Pagination.Execute(\n" +
                "                efEntities,\n" +
                "                this.paginationContext,\n" +
                "                efEntityDto => EntityDtoForPurpose.FromEfEntityDto(efEntityDto));\n" +
                "        }";
        }
    }
}