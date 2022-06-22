using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityDetailFromMethodsAddition : RelationAdditionEditor
    {
        public DbEntityDetailFromMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "System.Linq");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromEf" + relationSide.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));

            stringEditor.InsertLine(
                $"                {relationSide.Name} = ef{relationSide.Entity.Name}.{relationSide.Name}\n" +
                $"                    .Select(ef{relationSide.OtherEntity.Name} => Db{relationSide.OtherEntity.Name}\n" +
                $"                        .FromEf{relationSide.OtherEntity.Name}(ef{relationSide.OtherEntity.Name})),");

            return stringEditor.GetText();
        }
    }
}