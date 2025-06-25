using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Notifications;
using Hangfire;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Events;
using Infrastructure.Notifications;
using Infrastructure.Outbox;
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
        services.AddTransient<INotificationService, NotificationService>();

        AddDatabase(services, configuration);
        AddCaching(services, configuration);
        AddHealthChecks(services, configuration);
        AddMessaging(services);
        AddBackgroundJobs(services, configuration);
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddSingleton<IDBConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDistributedMemoryCache();
        string redisConnectionString = configuration.GetConnectionString("Cache")!;

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            //.AddCheck<DatabaseHealthCheck>("custom-sql", HealthStatus.Unhealthy) não usar
            .AddRedis(configuration.GetConnectionString("Cache")!)
            .AddSqlServer(configuration.GetConnectionString("Database")!)
            .AddDbContextCheck<ApplicationDbContext>();
    }

    private static void AddMessaging(IServiceCollection services)
    {
        services.AddSingleton<InMemoryMessageQueue>();
        services.AddTransient<IEventBus, EventBus>();
        services.AddHostedService<IntegrationEventProcessorJob>();
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
            config.UseSqlServerStorage(
                configuration.GetConnectionString("Database")));

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();
    }
}
