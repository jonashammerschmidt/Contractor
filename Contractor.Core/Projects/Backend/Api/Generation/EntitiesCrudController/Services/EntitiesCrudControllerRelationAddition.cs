using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Api
{
    internal class EntitiesCrudControllerRelationAddition : RelationAdditionEditor
    {
        public EntitiesCrudControllerRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            string paginationLineStart = "        [Pagination(FilterFields = new[] { ";
            stringEditor.NextThatContains(paginationLineStart);
            var paginationLineEnd = stringEditor.GetLine().Substring(paginationLineStart.Length);
            var updatedLine = paginationLineStart + "\"" + options.PropertyNameFrom + "Id\", " + paginationLineEnd;
            stringEditor.SetLine(updatedLine);

            return stringEditor.GetText();
        }
    }
}