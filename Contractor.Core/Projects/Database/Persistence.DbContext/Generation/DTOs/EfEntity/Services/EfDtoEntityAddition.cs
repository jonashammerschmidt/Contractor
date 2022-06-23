namespace Contractor.Core.Tools
{
    internal class EfDtoEntityAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EfDtoEntityAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Edit(Entity entity, string domainFolder, string templateFileName)
        {
            if (entity.IdType == "AutoIncrement")
            {
                string filePath = this.pathService.GetAbsolutePathForDatabase(entity, domainFolder, templateFileName);
                string fileData = UpdateFileData(entity, filePath);

                this.fileSystemClient.WriteAllText(fileData, filePath);
            }
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            fileData = fileData.Replace(
                "public Guid Id { get; set; }",
                "public int Id { get; set; }");

            return fileData;
        }
    }
}