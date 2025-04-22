using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Notifications;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Events;
using Infrastructure.Notifications;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Training.Domain.Activies;
using Modules.Training.Domain.Workouts;
using Modules.Users.Domain.Followers;
using Modules.Users.Domain.Users;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddSingleton<IDBConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));

        services.AddSingleton<PublishDomainEventsInterceptor>();
        services.AddSingleton<InsertOutboxMessageInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) => options
                .UseSqlServer(connectionString)
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageInterceptor>()));

        //services.AddDbContext<IApplicationReadDbContext, ApplicationReadDbContext>(
        //    options => options
        //    .UseSqlServer(connectionString)
        //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowerRepository, FollowerRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();

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
        services.AddSingleton<IEventBus, EventBus>();
        services.AddHostedService<IntegrationEventProcessorJob>();
    }
}
