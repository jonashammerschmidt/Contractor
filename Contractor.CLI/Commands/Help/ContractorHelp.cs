using Contractor.CLI.Tools;

namespace Contractor.CLI
{
    internal class ContractorHelp
    {
        public static void WriteHelp()
        {
            LogoWriter.Write();
            System.Console.WriteLine(
                @"Commands:
contractor init [-y]
contractor add domain Bankwesen
contractor add entity Bankwesen.Bank:Banken [-s|--scope Mandant:Mandanten]
contractor add property string:256 Name -e Bankwesen.Bank:Banken
contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden
            ");
        }
    }
}