using System.Data;
using Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

internal sealed class DbConnectionFactory(string connectionString) : IDBConnectionFactory
{
    public IDbConnection GetOpenConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}
