using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityDetailFromOneToOneMethodsAddition : RelationAdditionEditor
    {
        public DbEntityDetailFromOneToOneMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromEf" + relationSide.Entity.Name);
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {relationSide.OtherName} = Db{relationSide.OtherEntity.Name}.FromEf{relationSide.OtherEntity.Name}(ef{relationSide.Entity.Name}.{relationSide.OtherName}),");

            return stringEditor.GetText();
        }
    }
}