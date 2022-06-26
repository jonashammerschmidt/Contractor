using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    internal class DtoRelationAddition
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
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, namespacesToAdd);
        }

        public void AddRelationToDTOForDatabase(RelationSide relationSide, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            AddRelationToDTO(relationSide, domainFolder, templateFileName, false, true, namespacesToAdd);
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, params string[] namespacesToAdd)
        {
            this.AddRelationToDTO(relationSide, domainFolder, templateFileName, forInterface, false, namespacesToAdd);
        }

        public void AddRelationToDTO(RelationSide relationSide, string domainFolder, string templateFileName, bool forInterface, bool forDatabase, params string[] namespacesToAdd)
        {
            string filePath = (forDatabase) ?
                this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName) :
                this.pathService.GetAbsolutePathForBackend(relationSide, domainFolder, templateFileName);

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

            if (PropertyLine.ContainsProperty(fileData))
            {
                stringEditor.InsertNewLine();
            }

            string optionalText = (relationSide.IsOptional && relationSide.Type == "Guid") ? "?" : "";
            if (forInterface)
                stringEditor.InsertLine($"        {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");
            else
                stringEditor.InsertLine($"        public {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");

            return stringEditor.GetText();
        }
    }
}