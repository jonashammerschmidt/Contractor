using Contractor.Core.Csv;
using Contractor.Core.Csv.Sql;

namespace Contractor.CLI.Commands.Csv;

public static class CsvCommand
{
    public static void HandleCsvJob(string[] args)
    {
        if (args.Length < 2 || (args[1] != "insert" && args[1] != "export"))
        {
            Console.WriteLine("Es wurde kein korrekter Csv-Job ('insert', 'export') angegeben.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            Environment.Exit(1);
        }

        SqlOptions sqlOptions = SqlOptionsParser.ParseSqlOptions(args);
        if (sqlOptions == null)
        {
            Environment.Exit(1);
        }
        
        if (args[1] == "insert")
        {
            CsvInsertFacade.Insert(Directory.GetCurrentDirectory(), sqlOptions);
        } 
        else if (args[1] == "export")
        {
            CsvExportFacade.Export(Directory.GetCurrentDirectory(), sqlOptions);
        }
    }
}