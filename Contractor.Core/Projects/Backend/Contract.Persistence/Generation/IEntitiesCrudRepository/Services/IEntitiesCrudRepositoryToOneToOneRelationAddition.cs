using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    internal class IEntitiesCrudRepositoryToOneToOneRelationAddition : RelationAdditionEditor
    {
        public IEntitiesCrudRepositoryToOneToOneRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- Create Method -----------
            stringEditor.NextThatContains($"bool Does{options.EntityNameTo}Exist(");
            stringEditor.Next();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        bool Is{options.PropertyNameFrom}IdInUse(Guid {options.PropertyNameFrom.LowerFirstChar()}Id);");

            return stringEditor.GetText();
        }
    }
}