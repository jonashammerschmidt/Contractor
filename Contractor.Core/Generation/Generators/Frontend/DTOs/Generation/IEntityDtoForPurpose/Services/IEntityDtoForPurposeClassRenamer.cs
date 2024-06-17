using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.DTOs
{
    public class IEntityDtoForPurposeClassRenamer
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public IEntityDtoForPurposeClassRenamer(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void Rename(Entity entity, string dtoName, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(entity, paths);
            string fileData = fileSystemClient.ReadAllText(entity, filePath);

            fileData = fileData.Replace("I" + entity.Name + "DtoForPurpose", dtoName);

            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}