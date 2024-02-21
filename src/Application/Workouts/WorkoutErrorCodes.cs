namespace Application.Workouts;

public static class WorkoutErrorCodes
{
    public static class CreateWorkout
    {
        public const string MissingUserId = nameof(MissingUserId);
        public const string MissingName = nameof(MissingName);
        public const string NameTooLong = nameof(NameTooLong);
    }

    public static class RemoveWorkout
    {
        public const string MissingWorkoutId = nameof(MissingWorkoutId);
    }
}
