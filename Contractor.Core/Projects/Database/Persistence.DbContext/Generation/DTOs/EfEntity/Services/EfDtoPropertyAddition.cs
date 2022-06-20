using Contractor.Core.Helpers;

namespace Contractor.Core.Tools
{
    internal class EfDtoPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EfDtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(property, domainFolder, templateFileName);
            string fileData = UpdateFileData(property, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Property property, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            PropertyLine.FindStartingLineForNewProperty(fileData, property.Entity.Name, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (PropertyLine.ContainsProperty(filePath))
            {
                stringEditor.InsertNewLine();
            }

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(DatabaseEfDtoPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}