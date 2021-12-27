using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityDetailFromOneToOneMethodsAddition : RelationAdditionEditor
    {
        public DbEntityDetailFromOneToOneMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromEf" + options.EntityNameFrom);
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameTo} = Db{options.EntityNameTo}.FromEf{options.EntityNameTo}(ef{options.EntityNameFrom}.{options.PropertyNameTo}),");

            return stringEditor.GetText();
        }
    }
}