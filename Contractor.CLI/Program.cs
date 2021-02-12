using System;

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

                case "add":
                case "rename":
                case "remove":
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
            switch (args[1])
            {
                case "domain":
                    DomainHandler.Perform(args);
                    break;

                case "entity":
                    EntityHandler.Perform(args);
                    break;

                case "property":
                    PropertyHandler.Perform(args);
                    break;

                case "relation":
                    RelationHandler.Perform(args);
                    break;

                default:
                    Console.WriteLine("Der Typ '" + args[1] + "' ist ungültig.");
                    Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                    break;
            }
        }
    }
}