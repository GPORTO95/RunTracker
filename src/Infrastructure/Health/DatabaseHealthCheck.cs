using System.Data;
using Dapper;
using Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.Health;

internal sealed class DatabaseHealthCheck(DbConnectionFactory dbConnectionFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateOpenConnection();

            await connection.ExecuteScalarAsync("SELECT 1");

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
