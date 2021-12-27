using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailFromOneToOneMethodsAddition : RelationAdditionEditor
    {
        public EntityDetailFromOneToOneMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromDb" + options.EntityNameFrom);
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameTo} = {options.EntityNameTo}.FromDb{options.EntityNameTo}(db{options.EntityNameFrom}Detail.{options.PropertyNameTo}),");

            return stringEditor.GetText();
        }
    }
}