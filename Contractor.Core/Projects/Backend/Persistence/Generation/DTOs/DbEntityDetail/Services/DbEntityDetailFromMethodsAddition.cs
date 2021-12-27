using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityDetailFromMethodsAddition : RelationAdditionEditor
    {
        public DbEntityDetailFromMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + options.EntityNameFrom);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyNameTo} = ef{options.EntityNameFrom}.{options.PropertyNameTo}" +
                $".Select(ef{options.EntityNameTo} => Db{options.EntityNameTo}.FromEf{options.EntityNameTo}(ef{options.EntityNameTo})),");

            return stringEditor.GetText();
        }
    }
}