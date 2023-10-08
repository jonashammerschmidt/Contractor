using System;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;

namespace Contractor.Core.Csv.Sql
{
    public class SqlCsvImporter
    {
        private readonly SqlOptions sqlOptions;

        private readonly CultureInfo cultureInfo = new CultureInfo("de-DE");

        public SqlCsvImporter(SqlOptions sqlOptions)
        {
            this.sqlOptions = sqlOptions;
        }

        public void Import(string csvFilePath, string tableName)
        {
            Console.WriteLine($"Importiere {tableName}...");

            string connectionString = this.sqlOptions.GetConnectionString();
            var csvConfiguration = new CsvConfiguration(cultureInfo)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
            };

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.TableLock, null))
            using (var streamReader = new StreamReader(csvFilePath))
            using (var csvReader = new CsvReader(streamReader, csvConfiguration))
            {
                csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("NULL");
                csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("null");

                var dataTable = new DataTable();
                dataTable.Locale = cultureInfo;
                this.ApplySchemaToDataTable(sqlConnection, dataTable, tableName);

                bool containsByteArrayColumn = dataTable.Columns
                    .Cast<DataColumn>()
                    .Any(column => column.DataType == typeof(byte[]));

                if (containsByteArrayColumn)
                {
                    this.LoadManuallyIntoDataTable(csvReader, dataTable);
                }
                else
                {
                    using (var dr = new CsvDataReader(csvReader))
                    {
                        dataTable.Load(dr);
                    }
                }

                sqlConnection.Open();

                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.WriteToServer(dataTable);
            }
        }

        private void ApplySchemaToDataTable(SqlConnection sqlConnection, DataTable dataTable, string tableName)
        {
            string selectQuery = "SELECT TOP 1 * FROM " + tableName;
            SqlCommand sqlCmd = new SqlCommand(selectQuery, sqlConnection);

            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);
            sda.FillSchema(dataTable, SchemaType.Source);
        }

        private void LoadManuallyIntoDataTable(CsvReader csvReader, DataTable dataTable)
        {
            foreach (ExpandoObject record in csvReader.GetRecords<dynamic>())
            {
                var row = dataTable.NewRow();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object value = record.Single(property => property.Key == column.ColumnName).Value;
                    row[column.ColumnName] = this.ConvertStringToType(column.DataType, value);
                }

                dataTable.Rows.Add(row);
            }
        }

        private object ConvertStringToType(Type type, object value)
        {
            string stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue) || stringValue.Trim().ToLower() == "null")
            {
                return DBNull.Value;
            }

            stringValue = stringValue.Trim();
            switch (type.Name)
            {
                case "Guid":
                    return Guid.Parse(stringValue);

                case "String":
                    return stringValue;

                case "Boolean":
                    return bool.Parse(stringValue);

                case "Double":
                    return double.Parse(stringValue);

                case "Int32":
                    return int.Parse(stringValue);

                case "DateTime":
                    return DateTime.ParseExact(stringValue, "dd.MM.yyyy HH:mm:ss", cultureInfo);

                case "Byte[]":
                    return Convert.FromBase64String(stringValue);

                default:
                    throw new ApplicationException($"Dieser Datensatz konnte nicht von String zu {type.Name} konvertiert werden: {stringValue}");
            }
        }
    }
}