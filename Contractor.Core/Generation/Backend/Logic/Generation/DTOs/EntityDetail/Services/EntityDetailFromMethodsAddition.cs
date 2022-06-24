using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Logic
{
    internal class EntityDetailFromMethodsAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntityDetailFromMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "System.Linq");
            fileData = UsingStatements.Add(fileData, $"{relationSide.Entity.Module.Options.Paths.ProjectName}.Logic.Modules.{relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("FromDb" + relationSide.Entity.Name);
            stringEditor.NextUntil(line => line.Trim().Equals("};"));
            stringEditor.InsertLine(
                $"                {relationSide.Name} = db{relationSide.Entity.Name}Detail.{relationSide.Name}\n" +
                $"                    .Select(db{relationSide.OtherEntity.Name} => {relationSide.OtherEntity.Module.Name}.{relationSide.OtherEntity.NamePlural}.{relationSide.OtherEntity.Name}\n" +
                $"                        .FromDb{relationSide.OtherEntity.Name}(db{relationSide.OtherEntity.Name})),");

            return stringEditor.GetText();
        }
    }
}