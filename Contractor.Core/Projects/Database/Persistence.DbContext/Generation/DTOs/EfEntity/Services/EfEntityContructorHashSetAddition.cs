using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    internal class EfEntityContructorHashSetAddition : RelationAdditionEditor
    {
        public EfEntityContructorHashSetAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"public Ef{options.EntityNameFrom}()");
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            this.{options.PropertyNameTo} = new HashSet<Ef{options.EntityNameTo}>();");

            return stringEditor.GetText();
        }
    }
}