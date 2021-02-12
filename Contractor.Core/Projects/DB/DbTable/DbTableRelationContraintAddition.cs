using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class DbTableRelationContraintAddition
    {
        public PathService pathService;

        public DbTableRelationContraintAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddContraint(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            var entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDbDomain(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("CONSTRAINT [PK_");
            stringEditor.Next(line => !line.Contains("CONSTRAINT [FK_"));

            stringEditor.InsertLine(GetPropertyLine(options));

            return stringEditor.GetText();
        }

        private static string GetPropertyLine(IRelationAdditionOptions options)
        {
            return $"    CONSTRAINT [FK_{options.EntityNamePluralTo}_{options.EntityNameFrom}Id] FOREIGN KEY ([{options.EntityNameFrom}Id]) REFERENCES [dbo].[{options.EntityNamePluralFrom}] ([Id]),";
        }
    }
}