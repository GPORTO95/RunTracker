using Domain.Workouts;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class ExerciseRepository(ApplicationWriteDbContext context) : IExerciseRepository
{
    public void InsertRange(IEnumerable<Exercise> exercises)
    {
        context.Exercises.AddRange(exercises);
    }
}
