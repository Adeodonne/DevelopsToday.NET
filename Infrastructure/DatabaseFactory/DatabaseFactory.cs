using System.Data;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DatabaseFactory;

public class DatabaseFactory : IDatabaseFactory
{
    private readonly string _connectionString;

    public DatabaseFactory()
    {
        _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}