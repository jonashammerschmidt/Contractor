using Contractor.CLI.Commands._Helper;
using Contractor.CLI.Migration;
using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.MetaModell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Contractor.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ContractorHelp.WriteHelp();
                return;
            }

            switch (args[0])
            {
                case "init":
                    ContractorInitialize.PerformInitialize(args);
                    break;

                case "help":
                    ContractorHelp.WriteHelp();
                    break;

                case "test":
                    HandleExecuteJob(
                        (new string[] { "execute", @"..\Contractor.XML\contractor.xml" })
                        .Concat(args[1..])
                        .ToArray());
                    break;

                case "execute":
                    HandleExecuteJob(args);
                    break;

                case "migrate":
                    HandleMigrationJob(args);
                    break;

                default:
                    Console.WriteLine("Der Befehl '" + args[0] + "' konnte nicht gefunden werden.");
                    Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                    break;
            }
        }

        private static void HandleExecuteJob(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Der Pfad wurde nicht angegeben.");
                Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                return;
            }

            string contractorXmlFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), args[1]));
            var contractorXmlFileInfo = new FileInfo(contractorXmlFilePath);
            if (!contractorXmlFileInfo.Exists)
            {
                contractorXmlFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..", args[1]));
                contractorXmlFileInfo = new FileInfo(contractorXmlFilePath);

                if (!contractorXmlFileInfo.Exists)
                {
                    Console.WriteLine("Die angegebene Datei konnte nicht gefunden werden.");
                }
            }

            var contractorXmlDocument = new XmlDocument();
            contractorXmlDocument.Load(File.OpenRead(contractorXmlFilePath));
            var contractorXmlReader = new XmlNodeReader(contractorXmlDocument);

            var contractorXmlSerializer = new XmlSerializer(typeof(ContractorXml));
            ContractorXml contractorXml = (ContractorXml)contractorXmlSerializer.Deserialize(contractorXmlReader);

            if (Assembly.GetExecutingAssembly().GetName().Version.CompareTo(Version.Parse(contractorXml.MinContractorVersion)) < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Es muss mindestens die Contractor Version {0} verwendet werden.", contractorXml.MinContractorVersion);
                Console.WriteLine("");
                Console.WriteLine("Update-Befehl: dotnet tool update --global contractor");
                Console.WriteLine("");
                Console.ResetColor(); 
                return;
            }

            ContractorGenerationOptions contractorGenerationOptions = ContractorXmlConverter
                .ToContractorGenerationOptions(contractorXml, contractorXmlDocument, contractorXmlFileInfo.Directory.FullName);

            if (contractorXml.Includes is not null)
            {
                foreach (var include in contractorXml.Includes.Includes)
                {
                    string contractorIncludeXmlFilePath = Path.GetFullPath(Path.Combine(
                        contractorXmlFileInfo.Directory.FullName,
                        include.Src));

                    var contractorIncludeXmlDocument = new XmlDocument();
                    contractorIncludeXmlDocument.Load(File.OpenRead(contractorIncludeXmlFilePath));
                    XmlReader contractorIncludeXmlReader = new XmlNodeReader(contractorIncludeXmlDocument);

                    var contractorIncludeXmlSerializer = new XmlSerializer(typeof(ContractorIncludeXml));
                    ContractorIncludeXml contractorIncludeXml = (ContractorIncludeXml)contractorIncludeXmlSerializer.Deserialize(contractorIncludeXmlReader);

                    ContractorXmlConverter.AddToContractorGenerationOptions(contractorGenerationOptions, contractorIncludeXml, contractorIncludeXmlDocument);
                }
            }

            contractorGenerationOptions.AddLinks();

            TagArgumentParser.AddTags(args, contractorGenerationOptions);
            contractorGenerationOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine($"Started generation...");

            var contractorCoreApi = new ContractorCoreApi(contractorGenerationOptions);
            contractorCoreApi.Generate();

            stopwatch.Stop();
            Console.WriteLine($"Finished generation after {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            stopwatch.Start();
            Console.WriteLine($"Started saving...");

            contractorCoreApi.SaveChanges();

            stopwatch.Stop();
            Console.WriteLine($"Finished saving after {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void HandleMigrationJob(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Der Pfad wurde nicht angegeben.");
                Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                return;
            }

            string ps1FileFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), args[1]));
            if (!File.Exists(ps1FileFilePath))
            {
                Console.WriteLine("Die angegebene Datei konnte nicht gefunden werden.");
            }

            var file = File.ReadAllText(ps1FileFilePath);
            var lines = file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var filteredLines = lines
                .Select(line => line.Trim())
                .Where(line => line.StartsWith("contractor add"));

            IContractorOptions contractorOptions = ContractorOptionsLoader
                .Load(Directory.GetCurrentDirectory());

            contractorOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            var contractorXml = new ContractorXml();

            contractorXml.Paths = new PathsXml();
            contractorXml.Paths.BackendDestinationFolder = contractorOptions.BackendDestinationFolder;
            contractorXml.Paths.FrontendDestinationFolder = contractorOptions.DbProjectName;
            contractorXml.Paths.DbDestinationFolder = contractorOptions.DbProjectName;
            contractorXml.Paths.GeneratedProjectName = contractorOptions.DbProjectName;
            contractorXml.Paths.DbProjectName = contractorOptions.DbProjectName;
            contractorXml.Paths.DbContextName = String.Empty;

            contractorXml.Replacements = new ReplacementsXml();
            contractorXml.Replacements.Replacements = contractorOptions.Replacements
                .Select(replacements => new ReplacementXml()
                {
                    Pattern = replacements.Key,
                    ReplaceWith = replacements.Value,
                })
                .ToList();

            contractorXml.Modules = new ModulesXml()
            {
                Modules = new List<ModuleXml>(),
            };
            foreach (string line in filteredLines)
            {
                var lineArgs = line.Split(" ").ToList();
                lineArgs.RemoveAt(0);

                ContractorMigrator.Migrate(contractorXml, contractorOptions, lineArgs.ToArray());
            }

            XmlSerializer ser = new XmlSerializer(typeof(ContractorXml));
            var fileWriter = File.OpenWrite("contractor.xml");
            ser.Serialize(fileWriter, contractorXml);
            fileWriter.Close();
        }
    }
}