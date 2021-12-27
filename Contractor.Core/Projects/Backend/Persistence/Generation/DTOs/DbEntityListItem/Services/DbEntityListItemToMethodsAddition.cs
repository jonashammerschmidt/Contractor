using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityListItemToMethodsAddition : RelationAdditionEditor
    {
        public DbEntityListItemToMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + options.EntityNameTo);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyNameFrom} = Db{options.EntityNameFrom}.FromEf{options.EntityNameFrom}(ef{options.EntityNameTo}.{options.PropertyNameFrom}),");

            return stringEditor.GetText();
        }
    }
}