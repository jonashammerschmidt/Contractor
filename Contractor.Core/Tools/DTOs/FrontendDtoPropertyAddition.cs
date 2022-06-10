using Contractor.Core.Helpers;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendDtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForFrontend(property, domainFolder, templateFileName);
            string fileData = UpdateFileData(property, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName, string importStatementTypes, string importStatementPath)
        {
            string filePath = this.pathService.GetAbsolutePathForFrontend(property, domainFolder, templateFileName);
            string fileData = UpdateFileData(property, filePath);

            fileData = ImportStatements.Add(fileData, importStatementTypes, importStatementPath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Property property, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            if (!stringEditor.GetLine().Contains("export interface"))
            {
                stringEditor.NextThatContains($"export interface");
            }
            stringEditor.NextThatContains("}");

            stringEditor.InsertLine(FrontendDtoPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}