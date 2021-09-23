using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
                    Test.Testen();
                    break;

                case "execute":
                    HandleExecuteJob(args);
                    break;

                case "add":
                    HandleMainJobs(args);
                    break;

                default:
                    Console.WriteLine("Der Befehl '" + args[0] + "' konnte nicht gefunden werden.");
                    Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                    break;
            }
        }

        private static void HandleMainJobs(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Der Typ wurde nicht angegeben.");
                Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                return;
            }

            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
            ContractorExecuter.Execute(
                contractorCoreApi,
                ContractorOptionsLoader.Load(Directory.GetCurrentDirectory()),
                args);
            contractorCoreApi.SaveChanges();
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

            var file = File.ReadAllText(filePath);
            var lines = file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var filteredLines = lines
                .Select(line => line.Trim())
                .Where(line => line.StartsWith("contractor add"));

            IContractorOptions contractorOptions = ContractorOptionsLoader
                .Load(Directory.GetCurrentDirectory());

            contractorOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine($"Started generation...");

            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
            foreach (string line in filteredLines)
            {
                var lineArgs = line.Split(" ").ToList();
                lineArgs.RemoveAt(0);

                ContractorExecuter.Execute(
                    contractorCoreApi,
                    contractorOptions,
                    lineArgs.ToArray());
            }

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