using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using System;
using System.IO;

namespace Contractor.Core.Tools
{
    public class DomainDependencyProvider
    {
        public UsingStatementAddition usingStatementAddition;
        public PathService pathService;

        public DomainDependencyProvider(
            UsingStatementAddition usingStatementAddition,
            PathService pathService)
        {
            this.usingStatementAddition = usingStatementAddition;
            this.pathService = pathService;
        }

        public void UpdateDependencyProvider(DomainOptions options, string projectFolder, string fileName)
        {
            string filePath = GetFilePath(options, projectFolder, fileName);
            string fileData = UpdateFileData(options, filePath, projectFolder);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(DomainOptions options, string projectFolder, string fileName)
        {
            return Path.Combine(options.BackendDestinationFolder, options.ProjectName + projectFolder, fileName);
        }

        private string UpdateFileData(DomainOptions options, string filePath, string projectFolder)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = AddStartupMethod(fileData, options, projectFolder);
            fileData = AddGetStartupMethodCall(fileData, options);

            return fileData;
        }

        private string AddStartupMethod(string fileData, DomainOptions options, string projectFolder)
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
            if (projectFolder.Equals(".Logic"))
            {
                startupMethod = @"
        private static void StartupDomain(IServiceCollection services)
        {
        }";
            }
            else if(projectFolder.Equals(".Persistence"))
            {
                startupMethod = @"
        private static void StartupDomain(IServiceCollection services)
        {
        }";
            }

            return startupMethod.Replace("Domain", domain);
        }

        private string AddGetStartupMethodCall(string fileData, DomainOptions options)
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