using Contractor.Core.Tools;

namespace Contractor.Core.BaseClasses
{
    internal abstract class RelationSideAdditionToExisitingFileGeneration
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public RelationSideAdditionToExisitingFileGeneration(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddRelationSideToBackendFile(RelationSide relationSide, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForBackend(relationSide, fileName);
            AddRelationSideToFile(relationSide, filePath);
        }

        public void AddRelationSideToDatabaseFile(RelationSide relationSide, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(relationSide, fileName);
            AddRelationSideToFile(relationSide, filePath);
        }

        public void AddRelationSideToFrontendFile(RelationSide relationSide, string fileName)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(relationSide, fileName);
            AddRelationSideToFile(relationSide, filePath);
        }

        private void AddRelationSideToFile(RelationSide relationSide, string filePath)
        {
            string fileData = fileSystemClient.ReadAllText(relationSide, filePath);
            fileData = UpdateFileData(relationSide, fileData);

            fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected abstract string UpdateFileData(RelationSide relationSide, string fileData);
    }
}