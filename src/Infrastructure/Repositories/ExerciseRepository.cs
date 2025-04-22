using Infrastructure.Data;
using Modules.Training.Domain.Workouts;

namespace Infrastructure.Repositories;

internal sealed class ExerciseRepository(ApplicationDbContext context) : IExerciseRepository
{
    public void InsertRange(IEnumerable<Exercise> exercises)
    {
        context.Exercises.AddRange(exercises);
    }
}
