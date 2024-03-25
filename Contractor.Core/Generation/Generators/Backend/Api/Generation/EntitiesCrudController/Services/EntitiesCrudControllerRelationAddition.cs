using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Api
{
    public class EntitiesCrudControllerRelationAddition : RelationSideAdditionToExisitingFileGeneration
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
            var updatedLine = $"{paginationLineStart}\"{relationSide.Name}\", " + paginationLineEnd;
            stringEditor.SetLine(updatedLine);

            return stringEditor.GetText();
        }
    }
}