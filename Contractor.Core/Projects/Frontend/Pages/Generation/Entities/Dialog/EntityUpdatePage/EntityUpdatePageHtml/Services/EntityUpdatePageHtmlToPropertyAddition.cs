using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityUpdatePageHtmlToPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityUpdatePageHtmlToPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(entityOptions.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("</form>");

            stringEditor.InsertLine(GetLine(options));

            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }

        private string GetLine(IRelationAdditionOptions options)
        {
            return 
                $"            <app-search-dropdown [formGroupInstance]=\"{options.EntityNameLowerTo}UpdateForm\"\n" +
                $"                [formControlNameInstance]=\"'{options.PropertyNameFrom.LowerFirstChar()}Id'\" label=\"{options.PropertyNameFrom.ToReadable()}\" idExpr=\"id\" displayExpr=\"bezeichnung\"\n" +
                $"                required=\"true\" [dataSource]=\"{options.PropertyNameFrom.LowerFirstChar()}DataSource\" [initialItem]=\"selected{options.PropertyNameFrom}\">\n" +
                $"            </app-search-dropdown>";
        }
    }
}