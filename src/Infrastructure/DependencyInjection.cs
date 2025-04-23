using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Notifications;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Events;
using Infrastructure.Notifications;
using Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddSingleton<IDBConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddTransient<INotificationService, NotificationService>();

        //services.AddDistributedMemoryCache();
        string redisConnectionString = configuration.GetConnectionString("Cache")!;

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        services.AddSingleton<ICacheService, CacheService>();

        services.AddHealthChecks()
            //.AddCheck<DatabaseHealthCheck>("custom-sql", HealthStatus.Unhealthy) não usar
            .AddRedis(redisConnectionString)
            .AddSqlServer(connectionString)
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddSingleton<InMemoryMessageQueue>();
        services.AddTransient<IEventBus, EventBus>();
        services.AddHostedService<IntegrationEventProcessorJob>();
    }
}
