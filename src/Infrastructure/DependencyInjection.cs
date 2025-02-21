using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Application.Abstractions.Notifications;
using Domain.Followers;
using Domain.Users;
using Domain.Workouts;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Notifications;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
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

        services.AddDbContext<ApplicationReadDbContext>(
            options => options
            .UseSqlServer(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationWriteDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowerRepository, FollowerRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        services.AddTransient<INotificationService, NotificationService>();

        services.AddDistributedMemoryCache();

        services.AddSingleton<ICacheService, CacheService>();
    }
}
