namespace Modules.Training.Domain.Workouts;

public interface IExerciseRepository
{
    void InsertRange(IEnumerable<Exercise> exercises);
}
