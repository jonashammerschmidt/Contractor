using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Api
{
    internal class EntitiesCrudControllerRelationAddition : RelationAdditionEditor
    {
        public EntitiesCrudControllerRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            string paginationLineStart = "        [Pagination(FilterFields = new[] { ";
            stringEditor.NextThatContains(paginationLineStart);
            var paginationLineEnd = stringEditor.GetLine().Substring(paginationLineStart.Length);
            var updatedLine = paginationLineStart + "\"" + relationSide.Name + "Id\", " + paginationLineEnd;
            stringEditor.SetLine(updatedLine);

            return stringEditor.GetText();
        }
    }
}