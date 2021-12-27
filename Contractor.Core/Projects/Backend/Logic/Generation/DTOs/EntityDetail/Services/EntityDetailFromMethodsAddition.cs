using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailFromMethodsAddition : RelationAdditionEditor
    {
        public EntityDetailFromMethodsAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = UsingStatements.Add(fileData, "System.Linq");

            StringEditor stringEditor = new StringEditor(fileData);
         
            stringEditor.NextThatContains("FromDb" + options.EntityNameFrom);
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyNameTo} = db{options.EntityNameFrom}Detail.{options.PropertyNameTo}" +
                $".Select(db{options.EntityNameTo} => {options.EntityNameTo}.FromDb{options.EntityNameTo}(db{options.EntityNameTo})),");

            return stringEditor.GetText();
        }
    }
}