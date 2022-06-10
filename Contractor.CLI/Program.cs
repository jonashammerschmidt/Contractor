using Contractor.CLI.Commands._Helper;
using Contractor.CLI.Tools;
using Contractor.Core;
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
                    HandleExecuteJob(new string[] { "execute", "contractor.xml" });
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

            string filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), args[1]));
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Die angegebene Datei konnte nicht gefunden werden.");
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(File.OpenRead(filePath));
            XmlReader reader = new XmlNodeReader(xmlDocument);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContractorXml));
            ContractorXml contractorXml = (ContractorXml) xmlSerializer.Deserialize(reader);

            ContractorGenerationOptions contractorGenerationOptions = contractorXml.ToContractorGenerationOptions(xmlDocument);
            TagArgumentParser.AddTags(args, contractorGenerationOptions);
            contractorGenerationOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine($"Started generation...");

            ContractorCoreApi contractorCoreApi = new ContractorCoreApi(contractorGenerationOptions);
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