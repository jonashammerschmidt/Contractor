using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class IEntityUpdateMethodAddition : FrontendRelationAdditionEditor
    {
        public IEntityUpdateMethodAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static from{options.EntityNameTo}Detail");
            stringEditor.NextThatContains("return {");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine($"            {options.PropertyNameFrom.LowerFirstChar()}Id: i{options.EntityNameTo}Detail.{options.PropertyNameFrom.LowerFirstChar()}?.id,");

            return stringEditor.GetText();
        }
    }
}