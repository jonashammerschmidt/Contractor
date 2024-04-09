using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    public class DtoRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DtoRelationAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForBackend(relationSide, domainFolder, templateFileName);
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, filePath, namespacesToAdd);
        }

        public void AddRelationToDTOForBackendGenerated(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForBackendGenerated(relationSide, domainFolder, templateFileName);
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, filePath, namespacesToAdd);
        }

        public void AddRelationToDTOForBackendGenerated(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForBackendGenerated(relationSide, domainFolder, templateFileName);
            AddRelationToDTO(relationSide, domainFolder, templateFileName, forInterface, filePath, namespacesToAdd);
        }

        public void AddRelationToDTOForDatabase(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName);
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, filePath, namespacesToAdd);
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForBackend(relationSide, domainFolder, templateFileName);
            this.AddRelationToDTO(relationSide, domainFolder, templateFileName, forInterface, filePath, namespacesToAdd);
        }

        private void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, string filePath, params string[] namespacesToAdd)
        {
            string fileData = UpdateFileData(relationSide, filePath, forInterface);

            if (namespacesToAdd != null)
            {
                foreach (string namespaceToAdd in namespacesToAdd)
                {
                    fileData = UsingStatements.Add(fileData, namespaceToAdd);
                }
            }

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(RelationSide relationSide, string filePath, bool forInterface)
        {
            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);

            fileData = AddUsingStatements(relationSide, fileData);
            fileData = AddProperty(fileData, relationSide, forInterface);

            return fileData;
        }

        private string AddUsingStatements(RelationSide relationSide, string fileData)
        {
            if (relationSide.Type == "Guid")
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            if (relationSide.Type.Contains("Enumerable"))
            {
                fileData = UsingStatements.Add(fileData, "System.Collections.Generic");
            }

            return fileData;
        }

        private string AddProperty(string fileData, RelationSide relationSide, bool forInterface)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            PropertyLine.FindStartingLineForNewProperty(fileData, relationSide.Entity.Name, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (PropertyLine.ContainsProperty(fileData) && stringEditor.GetPrevLine().Trim().Length != 0)
            {
                stringEditor.InsertNewLine();
            }

            string optionalText = (relationSide.IsOptional && relationSide.Type == "Guid") ? "?" : "";
            if (forInterface)
                stringEditor.InsertLine($"        {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");
            else
                stringEditor.InsertLine($"        public {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");

            if (stringEditor.GetLine().Trim() != "}" && stringEditor.GetLine().Trim() != "")
            {
                stringEditor.InsertNewLine();
            }

            return stringEditor.GetText();
        }
    }
}