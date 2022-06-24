using Contractor.CLI.Commands._Helper;
using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.MetaModell;
using System;
using System.Diagnostics;
using System.IO;
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
                    HandleExecuteJob(new string[] { "execute", @"..\Contractor.XML\contractor.xml" });
                    break;

                case "execute":
                    HandleExecuteJob(args);
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
                Console.WriteLine("Der Typ wurde nicht angegeben.");
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
            ContractorXml contractorXml = (ContractorXml) contractorXmlSerializer.Deserialize(contractorXmlReader);

            ContractorGenerationOptions contractorGenerationOptions = ContractorXmlConverter
                .ToContractorGenerationOptions(contractorXml, contractorXmlDocument, contractorXmlFileInfo.Directory.FullName);

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
    }
}