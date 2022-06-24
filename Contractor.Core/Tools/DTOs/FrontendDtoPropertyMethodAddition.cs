using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoPropertyMethodAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendDtoPropertyMethodAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(Property property, string functionName, string variableName, string domainFolder, string fileName)
        {
            functionName = functionName.Replace("Entity", property.Entity.Name);
            functionName = functionName.Replace("entity", property.Entity.Name.LowerFirstChar());

            variableName = variableName.Replace("Entity", property.Entity.Name);
            variableName = variableName.Replace("entity", property.Entity.Name.LowerFirstChar());

            string filePath = this.pathService.GetAbsolutePathForFrontend(property, domainFolder, fileName);
            string fileData = UpdateFileData(property, functionName, variableName, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }


        private string UpdateFileData(Property property, string functionName, string variableName, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static " + functionName);
            stringEditor.NextThatContains("return {");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine($"            {property.Name.LowerFirstChar()}: {variableName}.{property.Name.LowerFirstChar()},");

            return stringEditor.GetText();
        }
    }
}