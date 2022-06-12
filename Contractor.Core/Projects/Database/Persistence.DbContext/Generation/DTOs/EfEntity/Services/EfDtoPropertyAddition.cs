using Contractor.Core.Helpers;
using Contractor.Core.Options;

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

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Property property, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            fileData = AddUsingStatements(property, fileData);
            fileData = AddProperty(fileData, property);

            return fileData;
        }

        private string AddUsingStatements(Property property, string fileData)
        {
            if (property.Type == PropertyTypes.Guid || property.Type == PropertyTypes.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            return fileData;
        }

        private string AddProperty(string file, Property property)
        {
            StringEditor stringEditor = new StringEditor(file);
            PropertyLine.FindStartingLineForNewProperty(file, property.Entity.Name, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (PropertyLine.ContainsProperty(file))
            {
                stringEditor.InsertNewLine();
            }

            stringEditor.InsertLine(DatabaseEfDtoPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}