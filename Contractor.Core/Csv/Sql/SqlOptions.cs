namespace Contractor.Core.Csv.Sql
{
    public class SqlOptions
    {
        public string ConnectionStringTemplate { get; set; }

        public string ServerAddress { get; set; }

        public string DatabaseName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int ConnectionTimeout { get; set; }

        public int CommandTimeout { get; set; }

        public string GetConnectionString()
        {
            string connectionStringTemplate = this.ConnectionStringTemplate;
            connectionStringTemplate = connectionStringTemplate.Replace("{{ServerAddress}}", this.ServerAddress);
            connectionStringTemplate = connectionStringTemplate.Replace("{{DatabaseName}}", this.DatabaseName);
            connectionStringTemplate = connectionStringTemplate.Replace("{{Username}}", this.Username);
            connectionStringTemplate = connectionStringTemplate.Replace("{{Password}}", this.Password);
            connectionStringTemplate = connectionStringTemplate.Replace("{{ConnectionTimeout}}", this.ConnectionTimeout.ToString());
            return connectionStringTemplate;
        }
    }
}