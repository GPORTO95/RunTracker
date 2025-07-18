﻿using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Training.Application.Abstractions.Data;
using Modules.Training.Domain.Activities;
using Modules.Training.Domain.Workouts;
using Modules.Training.Infrastructure.Database;
using Modules.Training.Infrastructure.Outbox;
using Modules.Training.Infrastructure.Repositories;
using SharedKernel;

namespace Modules.Training.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTrainingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<InsertOutboxMessageInterceptor>();

        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddDbContext<TrainingDbContext>(
            (sp, options) => options
                .UseSqlServer(connectionString, sql => sql.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Training))
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TrainingDbContext>());
        
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();

        return services;
    }
}
