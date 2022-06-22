using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EfDtoContructorHashSetAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EfDtoContructorHashSetAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(RelationSide relationSide, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName);
            string fileData = UpdateFileData(relationSide, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(RelationSide relationSide, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"public Ef{relationSide.Entity.Name}()");
            stringEditor.NextUntil(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            this.{relationSide.Name} = new HashSet<Ef{relationSide.OtherEntity.Name}>();");

            return stringEditor.GetText();
        }
    }
}