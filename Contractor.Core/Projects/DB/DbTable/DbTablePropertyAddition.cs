using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using System.IO;

namespace Contractor.Core.Tools
{
    public class DbTablePropertyAddition
    {
        public PathService pathService;

        public DbTablePropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddProperty(PropertyOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(PropertyOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDbDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(PropertyOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("PRIMARY KEY CLUSTERED");

            stringEditor.InsertLine(GetPropertyLine(options));
            
            return stringEditor.GetText();
        }

        private static string GetPropertyLine(PropertyOptions options)
        {
            // TODO: PropertyName length determines spaces betweeen name and type
            if (options.PropertyType == "string")
            {
                return $"	[{options.PropertyName}]	   NVARCHAR ({options.PropertyTypeExtra})   NOT NULL,";
            }
            else if (options.PropertyType == "int")
            {
                return $"	[{options.PropertyName}]	   INT              NOT NULL,";
            }
            else if (options.PropertyType == "Guid")
            {
                return $"	[{options.PropertyName}]	   UNIQUEIDENTIFIER NOT NULL,";
            }
            else if (options.PropertyType == "Guid?")
            {
                return $"	[{options.PropertyName}]	   UNIQUEIDENTIFIER NULL,";
            }
            else if (options.PropertyType == "bool")
            {
                return $"	[{options.PropertyName}]	   BIT              NOT NULL,";
            }
            else if (options.PropertyType == "DateTime")
            {
                return $"	[{options.PropertyName}]	   DATETIME         NOT NULL,";
            }
            

            return "-- TODO: ";
        }
    }
}