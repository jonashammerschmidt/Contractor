using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Persistence.DbContext
{
    internal class EfDtoPropertyAddition
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EfDtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDto(Property property, string domainFolder, string templateFileName)
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