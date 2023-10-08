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
contractor csv insert [-t|--login-type] [sql|integrated] [-s|--server-address] [-d|--database-name] [-u|--user] [-p|--password] [-v|--verbose]
contractor csv export [-t|--login-type] [sql|integrated] [-s|--server-address] [-d|--database-name] [-u|--user] [-p|--password] [-v|--verbose]
contractor migrate <relative-path>
            ");
        }
    }
}
