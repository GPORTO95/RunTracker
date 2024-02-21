namespace Infrastructure.Data.Models;

internal sealed class WorkoutReadModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<ExerciseReadModel> Exercises { get; set; }

    public UserReadModel User { get; set; }
}
