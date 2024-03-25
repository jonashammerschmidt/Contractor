using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    public class DtoInterfacePropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public DtoInterfacePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
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

            if (PropertyLine.ContainsProperty(fileData))
            {
                stringEditor.InsertNewLine();
            }

            stringEditor.InsertLine(BackendDtoInterfacePropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}