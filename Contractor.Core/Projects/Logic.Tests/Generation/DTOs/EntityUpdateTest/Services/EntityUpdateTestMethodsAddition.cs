using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
    internal class EntityUpdateTestMethodsAddition
    {
        public PathService pathService;

        public EntityUpdateTestMethodsAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static I{options.EntityName}Update ForUpdate()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}ForUpdate,");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}