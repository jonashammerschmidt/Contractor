using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class DtoPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public DtoPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            if (property.Type == PropertyType.Guid || property.Type == PropertyType.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            StringEditor stringEditor = new StringEditor(fileData);
            PropertyLine.FindStartingLineForNewProperty(fileData, property.Entity.Name, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (PropertyLine.ContainsProperty(fileData) && stringEditor.GetPrevLine().Trim().Length != 0)
            {
                stringEditor.InsertNewLine();
            }

            stringEditor.InsertLine(BackendDtoPropertyLine.GetPropertyLine(property));

            if (stringEditor.GetLine().Trim() != "}" && stringEditor.GetLine().Trim() != "")
            {
                stringEditor.InsertNewLine();
            }

            return stringEditor.GetText();
        }
    }
}