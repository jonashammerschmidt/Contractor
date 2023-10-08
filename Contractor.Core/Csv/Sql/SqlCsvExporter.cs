using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Contractor.Core.Csv.Sql
{
    public class SqlCsvExporter
    {
        public static void Export(string csvFilePath, string tableName, DataTable dataTable)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            CsvConfiguration csvConfiguration = new CsvConfiguration(new CultureInfo("de-DE"));
            csvConfiguration.Delimiter = ";";
            csvConfiguration.HasHeaderRecord = true;

            using (var writer = new StreamWriter(csvFilePath))
            using (var csvWriter = new CsvWriter(writer, csvConfiguration))
            {
                // Header
                foreach (DataColumn column in dataTable.Columns)
                {
                    csvWriter.WriteField(column.ColumnName);
                }
                csvWriter.NextRecord();

                // Data
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        csvWriter.WriteField(row[column] != DBNull.Value 
                            ? row[column]
                            : "null");
                    }
                    csvWriter.NextRecord();
                }
            }

            double dauer = Math.Round(stopwatch.Elapsed.TotalSeconds, 2);
            Console.WriteLine($"{tableName} erfolgreich exportiert. Dauer: {dauer}s");
        }
    }
}
