using Application.Abstractions.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationReadDbContext
{
    DbSet<UserReadModel> Users { get; }

    DbSet<FollowerReadModel> Followers { get; }

    DbSet<WorkoutReadModel> Workouts { get; }

    DbSet<ExerciseReadModel> Exercises { get; }
}

