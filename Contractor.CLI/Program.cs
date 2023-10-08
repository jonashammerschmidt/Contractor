using Contractor.CLI.Commands.Csv;

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
                    GenerateCommand.HandleExecuteJob(
                        (new string[] { "execute", @"..\Contractor.XML\contractor.xml" })
                        .Concat(args[1..])
                        .ToArray());
                    break;

                case "execute":
                    GenerateCommand.HandleExecuteJob(args);
                    break;

                case "migrate":
                    MigrateCommand.HandleMigrationJob(args);
                    break;

                case "csv":
                    CsvCommand.HandleCsvJob(args);
                    break;

                default:
                    Console.WriteLine("Der Befehl '" + args[0] + "' konnte nicht gefunden werden.");
                    Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                    break;
            }
        }
    }
}