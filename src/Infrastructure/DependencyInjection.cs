using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Application.Abstractions.Notifications;
using Domain.Followers;
using Domain.Users;
using Domain.Workouts;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Health;
using Infrastructure.Notifications;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

        services.AddTransient(_ => new DbConnectionFactory(connectionString));

        services.AddSingleton<PublishDomainEventsInterceptor>();
        services.AddSingleton<InsertOutboxMessageInterceptor>();

        services.AddDbContext<ApplicationWriteDbContext>(
            (sp, options) => options
                .UseSqlServer(connectionString)
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageInterceptor>()));

        services.AddDbContext<IApplicationReadDbContext, ApplicationReadDbContext>(
            options => options
            .UseSqlServer(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationWriteDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowerRepository, FollowerRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();

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
            .AddDbContextCheck<ApplicationWriteDbContext>();
    }
}
