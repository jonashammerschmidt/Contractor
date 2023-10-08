using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Contractor.Core.Csv.Sql;
using Microsoft.Data.SqlClient;

namespace Contractor.Core.Csv
{
    public static class CsvExportFacade
    {
        public static void Export(string path, SqlOptions sqlOptions)
        {
            string connectionString = sqlOptions.GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string[] tabellenNamen = GetTableNames(sqlConnection);
                foreach (string tabellenName in tabellenNamen)
                {
                    string csvFilePath = Directory.GetFiles(path, $"*{tabellenName}.csv", SearchOption.AllDirectories).FirstOrDefault();
                    if (csvFilePath == null)
                    {
                        csvFilePath = Path.Combine(path, "dbo." + tabellenName + ".csv");
                    }
                    else
                    {
                        File.Delete(csvFilePath);
                    }

                    DataTable dataTable = LoadDataTable(sqlConnection, tabellenName);
                    if (dataTable.Rows.Count > 0)
                    {
                        SqlCsvExporter.Export(csvFilePath, tabellenName, dataTable);
                    }
                }
            }
        }

        private static string[] GetTableNames(SqlConnection sqlConnection)
        {
            const string query = @"
                SELECT s.name + '.' + t.name AS TableName
                FROM sys.tables t 
                JOIN sys.schemas s 
                    ON t.schema_id = s.schema_id
                WHERE s.schema_id NOT IN (2,3,4) AND s.schema_id < 1000
                ORDER BY 1";

            List<string> tableNames = new List<string>();

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableNames.Add(reader["TableName"].ToString());
                    }
                }
            }

            return tableNames.ToArray();
        }


        private static DataTable LoadDataTable(SqlConnection sqlConnection, string tabellenName)
        {
            SqlCommand scom = sqlConnection.CreateCommand();
            scom.CommandText = $@"SELECT * FROM {tabellenName}";
            if (!tabellenName.Contains("__EFMigrationsHistory"))
            {
                scom.CommandText += $@" ORDER BY Id";
            }

            SqlDataReader sqlDataReader = scom.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            return dataTable;
        }
    }
}