using Contractor.Core.Helpers;
using Contractor.Core.Options;
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

        public void UpdateDependencyProvider(Module module, string projectFolder, string fileName)
        {
            string filePath = GetFilePath(module, projectFolder, fileName);
            string fileData = UpdateFileData(module, filePath, projectFolder);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(Module module, string projectFolder, string fileName)
        {
            return Path.Combine(module.Options.Paths.BackendDestinationFolder, projectFolder, fileName);
        }

        private string UpdateFileData(Module module, string filePath, string projectFolder)
        {
            string fileData = this.fileSystemClient.ReadAllText(module, filePath);

            fileData = AddStartupMethod(fileData, module, projectFolder);
            fileData = AddGetStartupMethodCall(fileData, module);

            return fileData;
        }

        private string AddStartupMethod(string fileData, Module module, string projectFolder)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // Insert using
            stringEditor.MoveToEnd();
            if (!stringEditor.GetLine().Contains("}"))
                stringEditor.PrevThatContains("}");

            stringEditor.PrevThatContains("}");
            stringEditor.InsertLine(GetStartupMethod(module.Name, projectFolder));

            return stringEditor.GetText();
        }

        private string GetStartupMethod(string moduleName, string projectFolder)
        {
            string startupMethod = "";
            if (projectFolder.Equals("Logic"))
            {
                startupMethod = @"
        private static void StartupDomain(IServiceCollection services)
        {
        }";
            }
            else if (projectFolder.Equals("Persistence"))
            {
                startupMethod = @"
        private static void StartupDomain(IServiceCollection services)
        {
        }";
            }

            return startupMethod.Replace("Domain", moduleName);
        }

        private string AddGetStartupMethodCall(string fileData, Module module)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // Insert using
            string startupMethodCall = GetStartupMethodCall(module.Name);
            stringEditor.NextThatContains("void Startup(IServiceCollection services");
            stringEditor.Next();
            stringEditor.Next(line => line.CompareTo(startupMethodCall) > 0 || line.Contains("}"));
            stringEditor.InsertLine(startupMethodCall);

            return stringEditor.GetText();
        }

        private string GetStartupMethodCall(string domain)
        {
            return $"            Startup{domain}(services);";
        }
    }
}