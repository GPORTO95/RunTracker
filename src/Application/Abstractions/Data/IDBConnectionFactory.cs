using System.Data;

namespace Application.Abstractions.Data;

public interface IDBConnectionFactory
{
    IDbConnection GetOpenConnection();
}
