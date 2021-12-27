using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class EntityTestValuesRelationAddition : RelationAdditionEditor
    {
        public EntityTestValuesRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.Next();
            stringEditor.PrevThatContains("}");
            stringEditor.PrevThatContains("}");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdDbDefault = {options.EntityNameFrom}TestValues.IdDbDefault;");
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdDbDefault2 = {options.EntityNameFrom}TestValues.IdDbDefault2;");
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdForCreate = {options.EntityNameFrom}TestValues.IdDbDefault;");
            stringEditor.InsertLine($"        public static readonly Guid {options.PropertyNameFrom}IdForUpdate = {options.EntityNameFrom}TestValues.IdDbDefault2;");

            return stringEditor.GetText();
        }
    }
}