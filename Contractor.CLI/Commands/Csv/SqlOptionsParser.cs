using Contractor.CLI.Tools;
using Contractor.Core.CsvInsert;

namespace Contractor.CLI.Commands.InsertCsv;

public class SqlOptionsParser
{
    public static SqlOptions ParseSqlOptions(string[] args)
    {
        SqlOptions sqlOptions = new SqlOptions()
        {
            CommandTimeout = 99999, 
            ConnectionTimeout = 10,
        };

        var username = Environment.GetEnvironmentVariable("localdb_user");
        var password = Environment.GetEnvironmentVariable("localdb_password");
        
        string loginType = (username != null && password != null)
            ? "sql"
            : ArgumentParser.ExtractArgument(args, "-t", "--login-type");
        if (loginType != "sql" && loginType != "integrated")
        {
            Console.WriteLine("Der Login Type [-l, --login-type] ist ungültig.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            return null;
        }

        if (loginType == "integrated")
        {
            sqlOptions.ConnectionStringTemplate =
                "Data Source={{ServerAddress}};Initial Catalog={{DatabaseName}};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True";
        }
        else if (loginType == "sql")
        {
            sqlOptions.ConnectionStringTemplate =
                "Server={{ServerAddress}};Database={{DatabaseName}};User Id={{Username}};Password={{Password}};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True";
        }

        sqlOptions.ServerAddress = Environment.GetEnvironmentVariable("localdb")
            ?? ArgumentParser.ExtractArgument(args, "-s", "--server-address");
        if (sqlOptions.ServerAddress == null)
        {
            Console.WriteLine("Die Server-Adresse (bzw. Data Source) [-s, --server-address] wurde nicht angegeben.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            return null;
        }
        
        sqlOptions.DatabaseName = ArgumentParser.ExtractArgument(args, "-d", "--database-name");
        if (sqlOptions.DatabaseName == null)
        {
            Console.WriteLine("Der Datenbankname [-d, --database-name] wurde nicht angegeben.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            return null;
        }

        if (loginType == "sql")
        {
            sqlOptions.Username = username ?? ArgumentParser.ExtractArgument(args, "-u", "--user");
            if (sqlOptions.Username == null)
            {
                Console.WriteLine("Der Nutzername [-u, --user] wurde nicht angegeben.");
                Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                return null;
            }
        
            sqlOptions.Password = password ?? ArgumentParser.ExtractArgument(args, "-p", "--password");
            if (sqlOptions.Password == null)
            {
                Console.WriteLine("Das Passwort [-p, --password] wurde nicht angegeben.");
                Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                return null;
            }
        }

        if (ArgumentParser.HasArgument(args, "-v"))
        {
            Console.WriteLine("ConnectionStringTemplate: " + sqlOptions.ConnectionStringTemplate);
            Console.WriteLine("ServerAddress: " + sqlOptions.ServerAddress);
            Console.WriteLine("DatabaseName: " + sqlOptions.DatabaseName);
            Console.WriteLine("Username: " + sqlOptions.Username);
            Console.WriteLine("Password: " + new string('*', sqlOptions.Password.Length));
            Console.WriteLine("ConnectionTimeout: " + sqlOptions.ConnectionTimeout);
            Console.WriteLine("CommandTimeout: " + sqlOptions.CommandTimeout);
        }

        return sqlOptions;
    }
}