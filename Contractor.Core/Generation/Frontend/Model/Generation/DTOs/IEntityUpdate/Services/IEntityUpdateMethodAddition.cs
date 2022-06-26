using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Model
{
    internal class IEntityUpdateMethodAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public IEntityUpdateMethodAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static from{relationSide.Entity.Name}Detail");
            stringEditor.NextThatContains("return {");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine($"            {relationSide.NameLower}Id: i{relationSide.Entity.Name}Detail.{relationSide.NameLower}?.id,");

            return stringEditor.GetText();
        }
    }
}