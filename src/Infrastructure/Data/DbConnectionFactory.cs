using Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data;

internal sealed class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateOpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
