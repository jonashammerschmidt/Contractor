using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public class ApiDtoPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public ApiDtoPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            if (property.Type == PropertyType.Guid || property.Type == PropertyType.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            fileData = UsingStatements.Add(fileData, "System.ComponentModel.DataAnnotations");

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

            if (!property.IsOptional)
            {
                stringEditor.InsertLine("        [Required]");
            }

            if (property.Type == PropertyType.String && property.TypeExtra != null && property.IsOptional)
            {
                stringEditor.InsertLine($"        [StringLength({property.TypeExtra})]");
            }

            if (property.Type == PropertyType.String && property.TypeExtra != null && !property.IsOptional)
            {
                stringEditor.InsertLine($"        [StringLength({property.TypeExtra}, MinimumLength = 1)]");
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