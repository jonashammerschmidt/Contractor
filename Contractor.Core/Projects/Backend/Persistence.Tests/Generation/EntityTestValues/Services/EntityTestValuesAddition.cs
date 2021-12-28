using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class EntityTestValuesAddition : PropertyAdditionEditor
    {
        public EntityTestValuesAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(IPropertyAdditionOptions options, string fileData)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{options.EntityName}.{options.PropertyName}"));

            // ----------- Asserts -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.Next();
            stringEditor.PrevThatContains("}");
            stringEditor.PrevThatContains("}");

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}DbDefault = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "DbDefault", random)};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}DbDefault2 = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "DbDefault2", random)};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}ForCreate = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "ForCreate", random)};");
            stringEditor.InsertLine($"        public static readonly {CSharpProperties.ToString(options.PropertyType)} {options.PropertyName}ForUpdate = " +
                $"{BackendEntityTestValuesPropertyLine.GetPropertyLine(options, "ForUpdate", random)};");

            return stringEditor.GetText();
        }
    }
}