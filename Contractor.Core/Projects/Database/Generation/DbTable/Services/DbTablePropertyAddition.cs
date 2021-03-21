using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database
{
    internal class DbTablePropertyAddition
    {
        public PathService pathService;

        public DbTablePropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddProperty(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDbDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = fileData.Replace("RequestScopes", options.RequestScopeNamePlural);
            fileData = fileData.Replace("RequestScope", options.RequestScopeName);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("PRIMARY KEY CLUSTERED");

            stringEditor.InsertLine(GetPropertyLine(options));

            return stringEditor.GetText();
        }

        private static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            string spaces = " ";
            int spaceCount = 20 - options.PropertyName.Length;
            for (int i = 0; i < spaceCount; i++)
            {
                spaces += " ";
            }

            if (options.PropertyType == "string")
            {
                return $"	[{options.PropertyName}]{spaces}NVARCHAR ({options.PropertyTypeExtra})   NOT NULL,";
            }
            else if (options.PropertyType == "int")
            {
                return $"	[{options.PropertyName}]{spaces}INT              NOT NULL,";
            }
            else if (options.PropertyType == "Guid")
            {
                return $"	[{options.PropertyName}]{spaces}UNIQUEIDENTIFIER NOT NULL,";
            }
            else if (options.PropertyType == "Guid?")
            {
                return $"	[{options.PropertyName}]{spaces}UNIQUEIDENTIFIER NULL,";
            }
            else if (options.PropertyType == "bool")
            {
                return $"	[{options.PropertyName}]{spaces}BIT              NOT NULL,";
            }
            else if (options.PropertyType == "DateTime")
            {
                return $"	[{options.PropertyName}]{spaces}DATETIME         NOT NULL,";
            }

            return $"-- TODO: {options.PropertyType} {options.PropertyName}";
        }
    }
}