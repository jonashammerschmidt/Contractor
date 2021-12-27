using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailToMethodsAddition : RelationAdditionEditor
    {
        public EntityDetailToMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromDb" + options.EntityNameTo);
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameFrom} = {options.DomainFrom}.{options.EntityNamePluralFrom}.{options.EntityNameFrom}" +
                $".FromDb{options.EntityNameFrom}(db{options.EntityNameTo}Detail.{options.PropertyNameFrom}),");

            return stringEditor.GetText();
        }
    }
}