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
contractor execute <relative-path> [-v|--verbose]
contractor add domain Bankwesen [-v|--verbose]
contractor add entity Bankwesen.Bank:Banken [-s|--scope Mandant:Mandanten] [-d|--display-property] [-v|--verbose]
contractor add property string:256 Name -e Bankwesen.Bank:Banken [-o|--optional] [-v|--verbose]
contractor add relation 1:1 Bankwesen.Bank:Banken Mitarbeiter.Chef:Chefs [-o|--optional] [-n|--alternative-property-names MeineBank:MeinChef] [-v|--verbose]
contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden [-o|--optional] [-n|--alternative-property-names Vertragsbank:Vertragskunden] [-v|--verbose]
            ");
        }
    }
}