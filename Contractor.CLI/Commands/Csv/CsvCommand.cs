using Contractor.Core.Csv.Sql;
using Contractor.Core.CsvExport;
using Contractor.Core.CsvInsert;

namespace Contractor.CLI.Commands.Csv;

public static class CsvCommand
{
    public static void HandleCsvJob(string[] args)
    {
        if (args.Length < 2 || (args[1] != "insert" && args[1] != "export"))
        {
            Console.WriteLine("Es wurde kein korrekter Csv-Job ('insert', 'export') angegeben.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            return;
        }

        SqlOptions sqlOptions = SqlOptionsParser.ParseSqlOptions(args);
        if (sqlOptions == null)
        {
            return;
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