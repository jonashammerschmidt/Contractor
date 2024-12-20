using System.Collections.Generic;
using System.IO;
using System.Linq;
using Contractor.Core.Csv.Sql;

namespace Contractor.Core.Csv
{
    public class CsvInsertFacade
    {
        public static void Insert(string path, SqlOptions sqlOptions)
        {
            SqlCsvImporter sqlCsvImporter = new SqlCsvImporter(sqlOptions);
            SqlDatabaseHelper sqlDatabaseHelper = new SqlDatabaseHelper(sqlOptions); 
            
            sqlDatabaseHelper.DisableConstraints();
            sqlDatabaseHelper.TruncateDatabase();

            var csvFiles = GetAllCsvFiles(path);
            foreach (var csvFile in csvFiles)
            {
                var filePath = csvFile.Split(Path.DirectorySeparatorChar);
                var fileName = filePath[^1];

                var entityNamePlural = fileName.Replace("dbo.", string.Empty);
                entityNamePlural = entityNamePlural.Replace(".csv", string.Empty);

                sqlCsvImporter.Import(csvFile, entityNamePlural);
            }

            sqlDatabaseHelper.EnableConstraints();
        }

        private static IEnumerable<string> GetAllCsvFiles(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var files = directoryInfo.GetFiles("*.csv", SearchOption.AllDirectories);
            return files.Select(file => file.FullName);
        }
    }
}