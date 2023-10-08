using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Contractor.Core.CsvInsert.Sql
{
    public class SqlDatabaseHelper
    {
        private readonly SqlOptions sqlOptions;

        public SqlDatabaseHelper(SqlOptions sqlOptions)
        {
            this.sqlOptions = sqlOptions;
        }

        public void DisableConstraints()
        {
            Console.WriteLine("Disabling all constraints...");
            this.ExecuteSqlCommands(new List<string>()
            {
                "EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'",
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
            });
        }

        public void EnableConstraints()
        {
            Console.WriteLine("Enabling all constraints...");
            this.ExecuteSqlCommands(new List<string>()
            {
                "EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'",
                "EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'",
            });
        }

        public void TruncateDatabase()
        {
            Console.WriteLine("Truncating database...");
            this.ExecuteSqlCommands(new List<string>()
            {
                "EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'",
            });
        }

        private void ExecuteSqlCommands(List<string> sqlCommands)
        {
            string connectionString = this.sqlOptions.GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (string cmdtext in sqlCommands)
                {
                    SqlCommand command = new SqlCommand(cmdtext, connection);
                    command.CommandTimeout = this.sqlOptions.CommandTimeout;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}