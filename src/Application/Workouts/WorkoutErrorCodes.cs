namespace Application.Workouts;

public static class WorkoutErrorCodes
{
    public static class Create
    {
        public const string MissingUserId = nameof(MissingUserId);
        public const string MissingName = nameof(MissingName);
        public const string NameTooLong = nameof(NameTooLong);
    }
}
