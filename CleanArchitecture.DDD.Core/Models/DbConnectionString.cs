using Microsoft.Data.SqlClient;
using System.Data;

namespace CleanArchitecture.DDD.Core.Models;

public class DbConnectionString(string connectionString)
{
    public bool IsReachable => IsValidDbConnectionString(connectionString);

    public string ConnectionString => connectionString;

    private static bool IsValidDbConnectionString(string connectionString)
    {
        static string RemoveDatabaseFromConnectionString(string connStr)
        {
            var separator = ";";

            return connStr
                .Split(separator)
                .Where(str => !str.Contains("Database="))
                .Aggregate((a, b) => a + separator + b);
        }

        if (string.IsNullOrWhiteSpace(connectionString))
            return false;

        connectionString = RemoveDatabaseFromConnectionString(connectionString);

        SqlConnection? connection = null;

        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            if (connection?.State == ConnectionState.Open)
                connection.Close();
        }

        return true;
    }
}

