using Contractor.CLI.Tools;

namespace Contractor.CLI
{
    public class ContractorHelp
    {
        public static void WriteHelp()
        {
            LogoWriter.Write();
            System.Console.WriteLine(
                @"Commands:
contractor init [-y]
contractor add domain Bankwesen
contractor add entity Bankwesen.Bank:Banken [-m | --for-mandant]
contractor add property string:256 Name -e Bankwesen.Bank:Banken
contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden
            ");
        }
    }
}