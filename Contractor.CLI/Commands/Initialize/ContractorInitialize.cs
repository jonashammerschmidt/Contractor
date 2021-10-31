using Contractor.CLI.Tools;
using Contractor.Core.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contractor.CLI
{
    internal class ContractorInitialize
    {
        public static void PerformInitialize(string[] args)
        {
            if (args.Contains("-y"))
                InitializeForce(Directory.GetCurrentDirectory());
            else
                Initialize(Directory.GetCurrentDirectory());
        }

        private static void Initialize(string currentFolder)
        {
            LogoWriter.Write();
            IContractorOptions options = InitializeOptions(currentFolder);
            Initialize(currentFolder, options);
        }

        private static void InitializeForce(string currentFolder)
        {
            LogoWriter.Write();
            IContractorOptions options = ContractorDefaultOptionsFinder.FindDefaultOptions(currentFolder);
            Initialize(currentFolder, options);
        }

        private static void Initialize(string currentFolder, IContractorOptions options)
        {
            options.BackendDestinationFolder = Path.GetRelativePath(currentFolder, options.BackendDestinationFolder);
            options.DbDestinationFolder = Path.GetRelativePath(currentFolder, options.DbDestinationFolder);
            options.FrontendDestinationFolder = Path.GetRelativePath(currentFolder, options.FrontendDestinationFolder);
            options.Replacements = new Dictionary<string, string>();
            string optionsJson = JsonConvert.SerializeObject(options, Formatting.Indented);
            string optionsPath = Path.Combine(currentFolder, "contractor.json");
            File.WriteAllText(optionsPath, optionsJson);

            System.Console.WriteLine("contractor.json wurde generiert.");
            System.Console.WriteLine();
        }

        private static IContractorOptions InitializeOptions(string currentFolder)
        {
            IContractorOptions defaultOptions = ContractorDefaultOptionsFinder.FindDefaultOptions(currentFolder);

            // BackendDestinationFolder
            System.Console.WriteLine($"Welches Backend-Projekt soll bearbeitet werden? (Empty = {defaultOptions.BackendDestinationFolder})");
            var userBackendDestinationFolder = System.Console.ReadLine().Trim();
            if (userBackendDestinationFolder.Length > 0)
            {
                defaultOptions.BackendDestinationFolder = userBackendDestinationFolder;
            }

            // DbDestinationFolder
            System.Console.WriteLine($"Welches Datenbank-Projekt soll bearbeitet werden? (Empty = {defaultOptions.DbDestinationFolder})");
            var userDbDestinationFolder = System.Console.ReadLine().Trim();
            if (userDbDestinationFolder.Length > 0)
            {
                defaultOptions.DbDestinationFolder = userDbDestinationFolder;
            }

            // FrontendDestinationFolder
            System.Console.WriteLine($"Welches Frontend-Projekt soll bearbeitet werden? (Empty = {defaultOptions.FrontendDestinationFolder})");
            var userFrontendDestinationFolder = System.Console.ReadLine().Trim();
            if (userFrontendDestinationFolder.Length > 0)
            {
                defaultOptions.FrontendDestinationFolder = userFrontendDestinationFolder;
            }

            // ProjectName
            System.Console.WriteLine($"Wie heißt das Backend-Projekt? (Empty = {defaultOptions.ProjectName})");
            var userProjectName = System.Console.ReadLine().Trim();
            if (userProjectName.Length > 0)
            {
                defaultOptions.ProjectName = userProjectName;
            }

            // DbProjectName
            System.Console.WriteLine($"Wie heißt das Datenbank-Projekt? (Empty = {defaultOptions.DbProjectName})");
            var userDbProjectName = System.Console.ReadLine().Trim();
            if (userDbProjectName.Length > 0)
            {
                defaultOptions.DbProjectName = userDbProjectName;
            }

            return defaultOptions;
        }
    }
}