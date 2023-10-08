using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class DomainDependencyProvider
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DomainDependencyProvider(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void UpdateDependencyProvider(Module module, string fileName)
        {
            string filePath = GetFilePath(module, fileName);
            string fileData = UpdateFileData(module, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string GetFilePath(Module module, string fileName)
        {
            return Path.Combine(module.Options.Paths.BackendDestinationFolder, fileName);
        }

        private string UpdateFileData(Module module, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(module, filePath);

            fileData = AddStartupMethod(fileData, module);
            fileData = AddGetStartupMethodCall(fileData, module);

            return fileData;
        }

        private string AddStartupMethod(string fileData, Module module)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // Insert using
            stringEditor.MoveToEnd();
            if (!stringEditor.GetLine().Contains("}"))
                stringEditor.PrevThatContains("}");

            stringEditor.PrevThatContains("}");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        private static void Startup{module.Name}(IServiceCollection services)");
            stringEditor.InsertLine("        {");
            stringEditor.InsertLine("        }");

            return stringEditor.GetText();
        }

        private string AddGetStartupMethodCall(string fileData, Module module)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // Insert using
            string startupMethodCall = GetStartupMethodCall(module.Name);
            stringEditor.NextThatContains("void Startup(IServiceCollection services");
            stringEditor.Next();
            stringEditor.NextUntil(line => line.CompareTo(startupMethodCall) > 0 || line.Contains("}"));
            stringEditor.InsertLine(startupMethodCall);

            return stringEditor.GetText();
        }

        private string GetStartupMethodCall(string domain)
        {
            return $"            Startup{domain}(services);";
        }
    }
}