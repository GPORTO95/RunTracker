using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Workouts;
using SharedKernel;

namespace Application.Workouts.Remove;

internal sealed class RemoveWorkoutCommandHandler : ICommandHandler<RemoveWorkoutCommand>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveWorkoutCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
    {
        _workoutRepository = workoutRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveWorkoutCommand request, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout is null)
        {
            return Result.Failure(WorkoutErrors.NotFound(request.WorkoutId));
        }

        _workoutRepository.Remove(workout);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
