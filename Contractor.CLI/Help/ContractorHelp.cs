using Colorful;
using Contractor.CLI.Tools;
using System.Drawing;

namespace Contractor.CLI
{
    public class ContractorHelp
    {
        public static void WriteHelp()
        {
            LogoWriter.Write();
            System.Console.WriteLine(
                @"contractor new <project-name>
contractor init [-y]

contractor add domain <domain-name>
contractor add entity <entity-name-or-path>[:<entity-name-plural>] [--domain <domain-name>] [--plural <entity-name-plural>]
contractor add property <property-type> <property-name> [--entity <entity-name-or-path>] [--extra=<extra>] [--domain <domain-name>] [--plural <entity-name-plural>]

contractor rename domain <domain-name-old>:<domain-name-new> [--to <domain-name-new>]
contractor rename entity <entity-name-old> [--to <entity-name-new>:<entity-name-plural-new>] [--domain <domain-name>]
contractor rename property <property-name-old>[:<property-name-new>] [--entity <entity-path>] [--to <property-name-new>]

contractor remove entity <entity-name-or-path> [--domain <domain-name>]
contractor remove property <property-path>
            ");
        }
    }
}