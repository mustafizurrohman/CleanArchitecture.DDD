using Microsoft.Data.SqlClient;
using System.Data;

namespace CleanArchitecture.DDD.Core.Models;

public class DbConnectionString
{
    private readonly string _connectionString;

    public bool IsReachable => IsValidDbConnectionString(_connectionString);

    public string ConnectionString => _connectionString;

    public DbConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    private static bool IsValidDbConnectionString(string connectionString)
    {
        static string RemoveDatabaseFromConnectionString(string connStr)
        {
            return connStr
                .Split(";")
                .Where(str => !str.Contains("Database="))
                .Aggregate((a, b) => a + ";" + b);
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

