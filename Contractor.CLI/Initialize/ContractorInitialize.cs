using Colorful;
using Newtonsoft.Json;
using Contractor.Core.Jobs;
using System.Drawing;
using System.IO;
using System.Linq;
using Contractor.CLI.Tools;

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
            IContractorOptions options = FindDefaultOptions(currentFolder);
            Initialize(currentFolder, options);
        }

        private static void Initialize(string currentFolder, IContractorOptions options)
        {
            options.BackendDestinationFolder = Path.GetRelativePath(currentFolder, options.BackendDestinationFolder);
            options.DbDestinationFolder = Path.GetRelativePath(currentFolder, options.DbDestinationFolder);
            string optionsJson = JsonConvert.SerializeObject(options, Formatting.Indented);
            string optionsPath = Path.Combine(currentFolder, "contractor.json");
            File.WriteAllText(optionsPath, optionsJson);

            System.Console.WriteLine("contractor.json wurde generiert.");
            System.Console.WriteLine();
        }

        private static IContractorOptions InitializeOptions(string currentFolder)
        {
            IContractorOptions defaultOptions = FindDefaultOptions(currentFolder);

            // BackendDestinationFolder
            System.Console.WriteLine($"Welches Backend-Projekt soll bearbeitet werden? (Empty = {defaultOptions.BackendDestinationFolder})");
            var userBackendDestinationFolder = System.Console.ReadLine().Trim();
            if (userBackendDestinationFolder.Length > 0)
            {
                defaultOptions.BackendDestinationFolder = userBackendDestinationFolder;
            }

            // BackendDestinationFolder
            System.Console.WriteLine($"Welches Datenbank-Projekt soll bearbeitet werden? (Empty = {defaultOptions.DbDestinationFolder})");
            var userDbDestinationFolder = System.Console.ReadLine().Trim();
            if (userDbDestinationFolder.Length > 0)
            {
                defaultOptions.DbDestinationFolder = userDbDestinationFolder;
            }

            // BackendDestinationFolder
            System.Console.WriteLine($"Wie heißt das Backend-Projekt? (Empty = {defaultOptions.ProjectName})");
            var userProjectName = System.Console.ReadLine().Trim();
            if (userProjectName.Length > 0)
            {
                defaultOptions.ProjectName = userProjectName;
            }

            // BackendDestinationFolder
            System.Console.WriteLine($"Wie heißt das Datenbank-Projekt? (Empty = {defaultOptions.DbProjectName})");
            var userDbProjectName = System.Console.ReadLine().Trim();
            if (userDbProjectName.Length > 0)
            {
                defaultOptions.DbProjectName = userDbProjectName;
            }

            return defaultOptions;
        }

        private static IContractorOptions FindDefaultOptions(string currentFolder)
        {
            string backendDestinationFolder = currentFolder;
            string dbDestinationFolder = FindBestDbDestinationFolder(currentFolder);
            string projectName = new DirectoryInfo(backendDestinationFolder).Name;
            string dbProjectName = new DirectoryInfo(dbDestinationFolder).Name;

            return new ContractorOptions()
            {
                BackendDestinationFolder = backendDestinationFolder,
                DbDestinationFolder = dbDestinationFolder,
                ProjectName = projectName,
                DbProjectName = dbProjectName
            };
        }

        private static string FindBestDbDestinationFolder(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);

            var path = FindDbDestinationFolder(di.FullName);
            if (path != null)
                return path;
            path = FindDbDestinationFolder(di.Parent.FullName);
            if (path != null)
                return path;
            path = FindDbDestinationFolder(di.Parent.Parent.FullName);
            if (path != null)
                return path;

            return null;
        }

        private static string FindDbDestinationFolder(string dir)
        {
            return Directory.GetDirectories(dir)
                            .Where(directory => new DirectoryInfo(directory).Name.EndsWith(".DB"))
                            .FirstOrDefault();
        }
    }
}