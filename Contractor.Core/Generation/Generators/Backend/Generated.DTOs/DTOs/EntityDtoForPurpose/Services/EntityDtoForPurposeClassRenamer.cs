using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class EntityDtoForPurposeClassRenamer
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EntityDtoForPurposeClassRenamer(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void Rename(Entity entity, string dtoName, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackendGenerated(entity, paths);
            string fileData = fileSystemClient.ReadAllText(entity, filePath);

            fileData = fileData.Replace(entity.Name + "DtoForPurpose", dtoName);

            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}