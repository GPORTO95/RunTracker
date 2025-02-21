using Domain.Workouts;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class WorkoutRepository(ApplicationWriteDbContext context) : IWorkoutRepository
{
    public Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Workouts.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public void Insert(Workout workout)
    {
        context.Workouts.Add(workout);
    }

    public void Remove(Workout workout)
    {
        context.Workouts.Remove(workout);
    }
}
