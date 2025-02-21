using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Workouts;
using SharedKernel;

namespace Application.Workouts.AddExercises;

internal sealed class AddExercisesCommandHandler : ICommandHandler<AddExercisesCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWorkoutRepository _workoutRepository;

    public async Task<Result> Handle(AddExercisesCommand request, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout is null)
        {
            return Result.Failure(WorkoutErrors.NotFound(request.WorkoutId));
        }

        var results = request
            .Exercises
            .Select(exercise => workout.AddExercise(
                exercise.ExerciseType,
                exercise.TargetType,
                exercise.DistanceMeters,
                exercise.DurationInSeconds))
            .ToList();

        if (results.Any(r => r.IsFailure))
        {
            return Result.Failure(ValidationError.FromResults(results));
        }

        _exerciseRepository.InsertRange(workout.Exercises);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
