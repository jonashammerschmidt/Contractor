using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    internal class IEntitiesCrudRepositoryToOneToOneRelationAddition : RelationAdditionEditor
    {
        public IEntitiesCrudRepositoryToOneToOneRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"bool Does{relationSide.Entity.Name}Exist(");
            stringEditor.Next();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        bool Is{relationSide.OtherName}InUse(Guid {relationSide.OtherNameLower}Id);");

            return stringEditor.GetText();
        }
    }
}