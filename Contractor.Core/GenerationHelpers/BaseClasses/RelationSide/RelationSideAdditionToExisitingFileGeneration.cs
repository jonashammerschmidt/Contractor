using Contractor.Core.MetaModell;
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

        public void AddRelationSideToBackendFile(RelationSide relationSide, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackend(relationSide, paths);
            AddRelationSideToFile(relationSide, filePath);
        }

        public void AddRelationSideToBackendGeneratedFile(RelationSide relationSide, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackendGenerated(relationSide, paths);
            AddRelationSideToFile(relationSide, filePath);
        }

        public void AddRelationSideToDatabaseFile(RelationSide relationSide, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForDatabase(relationSide, paths);
            AddRelationSideToFile(relationSide, filePath);
        }

        public void AddRelationSideToFrontendFile(RelationSide relationSide, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(relationSide, paths);
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