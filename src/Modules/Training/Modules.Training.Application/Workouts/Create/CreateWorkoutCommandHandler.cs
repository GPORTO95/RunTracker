using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Modules.Training.Domain.Workouts;
using Modules.Users.Domain.Users;
using SharedKernel;

namespace Modules.Training.Application.Workouts.Create;

internal sealed class CreateWorkoutCommandHandler(
    IUserRepository userRepository,
    IWorkoutRepository workoutRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateWorkoutCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(request.UserId));
        }

        Workout workout = new(Guid.NewGuid(), user.Id, request.Name);

        workoutRepository.Insert(workout);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return workout.Id;
    }
}
