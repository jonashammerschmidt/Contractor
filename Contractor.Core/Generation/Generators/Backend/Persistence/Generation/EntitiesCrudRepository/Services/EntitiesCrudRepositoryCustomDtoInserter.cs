using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntitiesCrudRepositoryCustomDtoInserter
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EntitiesCrudRepositoryCustomDtoInserter(IFileSystemClient fileSystemClient, PathService pathService)
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