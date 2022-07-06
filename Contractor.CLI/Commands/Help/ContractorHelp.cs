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
contractor migrate <relative-path>
            ");
        }
    }
}