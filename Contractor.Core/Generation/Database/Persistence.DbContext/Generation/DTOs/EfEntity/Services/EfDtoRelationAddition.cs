using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Database.Persistence.DbContext
{
    internal class EfDtoRelationAddition
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EfDtoRelationAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddRelationToDto(RelationSide relationSide, string domainFolder, string templateFileName, bool addGraphQlAttributes, bool addGraphQlListAttributes, params string[] namespacesToAdd)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName);

            string fileData = UpdateFileData(relationSide, filePath, addGraphQlAttributes, addGraphQlListAttributes);

            if (addGraphQlAttributes)
            {
                fileData = UsingStatements.Add(fileData, "HotChocolate");
                fileData = UsingStatements.Add(fileData, "HotChocolate.Data");
                fileData = UsingStatements.Add(fileData, "HotChocolate.Types");
            }

            if (namespacesToAdd != null)
            {
                foreach (string namespaceToAdd in namespacesToAdd)
                {
                    fileData = UsingStatements.Add(fileData, namespaceToAdd);
                }
            }

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(RelationSide relationSide, string filePath, bool addGraphQlAttributes, bool addGraphQlListAttributes)
        {
            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);

            fileData = AddUsingStatements(relationSide, fileData);
            fileData = AddProperty(fileData, relationSide, addGraphQlAttributes, addGraphQlListAttributes);

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

        private string AddProperty(string fileData, RelationSide relationSide, bool addGraphQlAttributes, bool addGraphQlListAttributes)
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

            if (addGraphQlListAttributes)
            {
                stringEditor.InsertLine($"        [UsePaging]");
            }

            if (addGraphQlAttributes)
            {
                stringEditor.InsertLine($"        [UseProjection]");
            }

            if (addGraphQlListAttributes)
            {
                stringEditor.InsertLine($"        [UseFiltering]");
                stringEditor.InsertLine($"        [UseSorting]");
            }

            string optionalText = relationSide.IsOptional ? "?" : "";
            stringEditor.InsertLine($"        public {relationSide.Type}{optionalText} {relationSide.Name} {{ get; set; }}");

            return stringEditor.GetText();
        }
    }
}