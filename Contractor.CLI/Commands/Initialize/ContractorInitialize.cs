using Contractor.CLI.Tools;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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
            ContractorXml contractorXml = InitializeOptions(currentFolder);
            Initialize(currentFolder, contractorXml);
        }

        private static void InitializeForce(string currentFolder)
        {
            LogoWriter.Write();
            ContractorXml contractorXml = ContractorDefaultOptionsFinder.FindDefaultOptions(currentFolder);
            Initialize(currentFolder, contractorXml);
        }

        private static void Initialize(string currentFolder, ContractorXml contractorXml)
        {
            contractorXml.Paths.BackendDestinationFolder = Path.GetRelativePath(currentFolder, contractorXml.Paths.BackendDestinationFolder);
            contractorXml.Paths.DbDestinationFolder = Path.GetRelativePath(currentFolder, contractorXml.Paths.DbDestinationFolder);
            contractorXml.Paths.FrontendDestinationFolder = Path.GetRelativePath(currentFolder, contractorXml.Paths.FrontendDestinationFolder);

            string optionsPath = Path.Combine(currentFolder, "contractor.xml");
            using (Stream fileWriterStream = File.OpenWrite(optionsPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContractorXml));
                xmlSerializer.Serialize(fileWriterStream, contractorXml);
            }

            System.Console.WriteLine("contractor.xml wurde generiert.");
            System.Console.WriteLine();
        }

        private static ContractorXml InitializeOptions(string currentFolder)
        {
            ContractorXml defaultOptions = ContractorDefaultOptionsFinder.FindDefaultOptions(currentFolder);

            // BackendDestinationFolder
            System.Console.WriteLine($"Welches Backend-Projekt soll bearbeitet werden? (Empty = {defaultOptions.Paths.BackendDestinationFolder})");
            var userBackendDestinationFolder = System.Console.ReadLine().Trim();
            if (userBackendDestinationFolder.Length > 0)
            {
                defaultOptions.Paths.BackendDestinationFolder = userBackendDestinationFolder;
            }

            // DbDestinationFolder
            System.Console.WriteLine($"Welches Datenbank-Projekt soll bearbeitet werden? (Empty = {defaultOptions.Paths.DbDestinationFolder})");
            var userDbDestinationFolder = System.Console.ReadLine().Trim();
            if (userDbDestinationFolder.Length > 0)
            {
                defaultOptions.Paths.DbDestinationFolder = userDbDestinationFolder;
            }

            // FrontendDestinationFolder
            System.Console.WriteLine($"Welches Frontend-Projekt soll bearbeitet werden? (Empty = {defaultOptions.Paths.FrontendDestinationFolder})");
            var userFrontendDestinationFolder = System.Console.ReadLine().Trim();
            if (userFrontendDestinationFolder.Length > 0)
            {
                defaultOptions.Paths.FrontendDestinationFolder = userFrontendDestinationFolder;
            }

            // ProjectName
            System.Console.WriteLine($"Wie heißt das Backend-Projekt? (Empty = {defaultOptions.Paths.ProjectName})");
            var userProjectName = System.Console.ReadLine().Trim();
            if (userProjectName.Length > 0)
            {
                defaultOptions.Paths.ProjectName = userProjectName;
            }

            // GeneratedProjectName
            System.Console.WriteLine($"Wie heißt das Generated-Backend-Projekt? (Empty = {defaultOptions.Paths.GeneratedProjectName})");
            var userGeneratedProjectName = System.Console.ReadLine().Trim();
            if (userGeneratedProjectName.Length > 0)
            {
                defaultOptions.Paths.GeneratedProjectName = userGeneratedProjectName;
            }

            // DbProjectName
            System.Console.WriteLine($"Wie heißt das Datenbank-Projekt? (Empty = {defaultOptions.Paths.DbProjectName})");
            var userDbProjectName = System.Console.ReadLine().Trim();
            if (userDbProjectName.Length > 0)
            {
                defaultOptions.Paths.DbProjectName = userDbProjectName;
            }

            // DbContextName
            System.Console.WriteLine($"Wie heißt der Datenbank-Context? (Empty = {defaultOptions.Paths.DbContextName})");
            var userDbContextName = System.Console.ReadLine().Trim();
            if (userDbContextName.Length > 0)
            {
                defaultOptions.Paths.DbContextName = userDbContextName;
            }

            return defaultOptions;
        }
    }
}