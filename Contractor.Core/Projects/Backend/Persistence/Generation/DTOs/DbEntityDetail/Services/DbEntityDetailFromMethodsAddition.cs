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
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {relationSide.OtherName} = ef{relationSide.Entity.Name}.{relationSide.OtherName}" +
                $".Select(ef{relationSide.OtherEntity.Name} => Db{relationSide.OtherEntity.Name}.FromEf{relationSide.OtherEntity.Name}(ef{relationSide.OtherEntity.Name})),");

            return stringEditor.GetText();
        }
    }
}