using Domain.Workouts;

namespace Application.Workouts.AddExercises;

public sealed record ExerciseRequest(
    ExerciseType ExerciseType,
    TargetType TargetType,
    decimal? DistanceMeters,
    int? DurationInSeconds);
