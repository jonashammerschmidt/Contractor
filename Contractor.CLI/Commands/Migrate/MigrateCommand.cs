using System.Xml.Serialization;
using Contractor.CLI.Migration;
using Contractor.CLI.Tools;

namespace Contractor.CLI;

public class MigrateCommand
{
    public static void HandleMigrationJob(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Der Pfad wurde nicht angegeben.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            return;
        }

        string ps1FileFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), args[1]));
        if (!File.Exists(ps1FileFilePath))
        {
            Console.WriteLine("Die angegebene Datei konnte nicht gefunden werden.");
        }

        var file = File.ReadAllText(ps1FileFilePath);
        var lines = file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var filteredLines = lines
            .Select(line => line.Trim())
            .Where(line => line.StartsWith("contractor add"));

        IContractorOptions contractorOptions = ContractorOptionsLoader
            .Load(Directory.GetCurrentDirectory());

        contractorOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

        var contractorXml = new ContractorXml();

        contractorXml.Paths = new PathsXml();
        contractorXml.Paths.BackendDestinationFolder = contractorOptions.BackendDestinationFolder;
        contractorXml.Paths.FrontendDestinationFolder = contractorOptions.DbProjectName;
        contractorXml.Paths.DbDestinationFolder = contractorOptions.DbProjectName;
        contractorXml.Paths.GeneratedProjectName = contractorOptions.DbProjectName;
        contractorXml.Paths.DbProjectName = contractorOptions.DbProjectName;
        contractorXml.Paths.DbContextName = String.Empty;

        contractorXml.Replacements = new ReplacementsXml();
        contractorXml.Replacements.Replacements = contractorOptions.Replacements
            .Select(replacements => new ReplacementXml()
            {
                Pattern = replacements.Key,
                ReplaceWith = replacements.Value,
            })
            .ToList();

        contractorXml.Modules = new ModulesXml()
        {
            Modules = new List<ModuleXml>(),
        };
        foreach (string line in filteredLines)
        {
            var lineArgs = line.Split(" ").ToList();
            lineArgs.RemoveAt(0);

            ContractorMigrator.Migrate(contractorXml, contractorOptions, lineArgs.ToArray());
        }

        XmlSerializer ser = new XmlSerializer(typeof(ContractorXml));
        var fileWriter = File.OpenWrite("contractor.xml");
        ser.Serialize(fileWriter, contractorXml);
        fileWriter.Close();
    }
}