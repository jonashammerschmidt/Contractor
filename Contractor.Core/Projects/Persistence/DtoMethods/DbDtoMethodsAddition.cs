using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class DbDtoMethodsAddition
    {
        public PathService pathService;

        public DbDtoMethodsAddition(PathService pathService)
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

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateEf" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            ef{options.EntityName}.{options.PropertyName} = db{options.EntityName}.{options.PropertyName};");
            fileData = stringEditor.GetText();

            // ----------- DbSet -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = ef{options.EntityName}.{options.PropertyName},");
            fileData = stringEditor.GetText();

            // ----------- DbSet -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("ToEf" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = db{options.EntityName}.{options.PropertyName},");

            return stringEditor.GetText();
        }
    }
}