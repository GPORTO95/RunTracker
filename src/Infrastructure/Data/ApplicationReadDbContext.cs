using Application.Abstractions.Data;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

internal sealed class ApplicationReadDbContext : DbContext, IUnitOfWork
{
    public ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserReadModel> Users { get; set; }

    public DbSet<FollowerReadModel> Followers { get; set; }

    public DbSet<WorkoutReadModel> Workouts { get; set; }

    public DbSet<ExerciseReadModel> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationReadDbContext).Assembly,
            WriteConfigurationsFilter);
    }

    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Configurations.Read") ?? false;
}
