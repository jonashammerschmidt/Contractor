using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System;
using System.Linq;

namespace Contractor.Core.Projects.Backend.Misc
{
    internal class E2ePostmanPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public E2ePostmanPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            Random random = new Random(IntHash.ComputeIntHash($"E2E {property.Entity.Name}.{property.Name}"));
            string createPropertyValue = TestValueGeneration.GetPropertyLine(property, "E2E ", " 1", random).Replace("\"", "\\\"");
            string createPropertyValue2 = TestValueGeneration.GetPropertyLine(property, "E2E ", " 2", random).Replace("\"", "\\\"");
            string updatePropertyValue = TestValueGeneration.GetPropertyLine(property, "Updated E2E ", " 1", random).Replace("\"", "\\\""); ;

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatStartsWith("\t\"item\": [");
            stringEditor.NextThatStartsWith($"\t\t\t\"name\": \"{property.Entity.Module.Name} - {property.Entity.NamePlural}\",");

            InsertCreateLine(property, createPropertyValue, stringEditor, string.Empty);

            InsertGetSingleLine(property, createPropertyValue, stringEditor);

            InsertGetPaggedLine(property, createPropertyValue, stringEditor);
            
            InsertUpdateLine(property, updatePropertyValue, stringEditor);

            InsertGetSingleLine(property, updatePropertyValue, stringEditor);

            InsertCreateLine(property, createPropertyValue, stringEditor, " 1");

            InsertCreateLine(property, createPropertyValue2, stringEditor, " 2");

            return stringEditor.GetText();
        }

        private static void InsertCreateLine(Property property, string createPropertyValue, StringEditor stringEditor, string postfix)
        {
            stringEditor.NextThatStartsWith($"\t\t\t\t\t\"name\": \"Create {property.Entity.Name + postfix}\",");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\"body\": {");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\t\"raw\": \"{\\n");
            if (stringEditor.GetLine().Split(':').Count() > 2)
            {
                stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $",");
            }
            stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $"\\n  \\\"{property.NameLower}\\\": {createPropertyValue}");
        }

        private static void InsertGetSingleLine(Property property, string propertyValue, StringEditor stringEditor)
        {
            stringEditor.NextThatStartsWith($"\t\t\t\t\t\"name\": \"Get {property.Entity.Name}\",");
            stringEditor.NextThatContains("var jsonData = pm.response.json();");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"\t\t\t\t\t\t\t\t\t\"    pm.expect(jsonData.{property.NameLower}).to.eql({propertyValue});\\r\",");
        }

        private static void InsertGetPaggedLine(Property property, string createPropertyValue, StringEditor stringEditor)
        {
            stringEditor.NextThatStartsWith($"\t\t\t\t\t\"name\": \"Get {property.Entity.NamePlural}\",");
            stringEditor.NextThatContains("var jsonData = pm.response.json();");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"\t\t\t\t\t\t\t\t\t\"    pm.expect(jsonData.data[0].{property.NameLower}).to.eql({createPropertyValue});\\r\",");
        }

        private static void InsertUpdateLine(Property property, string updatePropertyValue, StringEditor stringEditor)
        {
            stringEditor.NextThatStartsWith($"\t\t\t\t\t\"name\": \"Update {property.Entity.Name}\",");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\"body\": {");
            stringEditor.NextThatStartsWith("\t\t\t\t\t\t\t\"raw\": \"{\\n");
            stringEditor.InsertIntoLine(stringEditor.GetLine().Length - 5, $",\\n  \\\"{property.NameLower}\\\": {updatePropertyValue}");
        }
    }
}