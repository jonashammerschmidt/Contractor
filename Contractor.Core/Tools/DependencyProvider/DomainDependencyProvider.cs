using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class DomainDependencyProvider
    {
        public PathService pathService;

        public DomainDependencyProvider(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void UpdateDependencyProvider(IDomainAdditionOptions options, string projectFolder, string fileName)
        {
            string filePath = GetFilePath(options, projectFolder, fileName);
            string fileData = UpdateFileData(options, filePath, projectFolder);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IDomainAdditionOptions options, string projectFolder, string fileName)
        {
            return Path.Combine(options.BackendDestinationFolder, projectFolder, fileName);
        }

        private string UpdateFileData(IDomainAdditionOptions options, string filePath, string projectFolder)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = AddStartupMethod(fileData, options, projectFolder);
            fileData = AddGetStartupMethodCall(fileData, options);

            return fileData;
        }

        private string AddStartupMethod(string fileData, IDomainAdditionOptions options, string projectFolder)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // Insert using
            stringEditor.MoveToEnd();
            if (!stringEditor.GetLine().Contains("}"))
                stringEditor.PrevThatContains("}");

            stringEditor.PrevThatContains("}");
            stringEditor.InsertLine(GetStartupMethod(options.Domain, projectFolder));

            return stringEditor.GetText();
        }

        private string GetStartupMethod(string domain, string projectFolder)
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

            return startupMethod.Replace("Domain", domain);
        }

        private string AddGetStartupMethodCall(string fileData, IDomainAdditionOptions options)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            // Insert using
            string startupMethodCall = GetStartupMethodCall(options.Domain);
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