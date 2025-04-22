using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Modules.Users.Domain.Followers;
using Modules.Users.Domain.Users;
using SharedKernel;

namespace Modules.Users.Application.Followers.StartFollowing;

internal sealed class StartFollowingCommandHandler : ICommandHandler<StartFollowingCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IFollowerService _followerService;
    private readonly IUnitOfWork _unitOfWork;

    public StartFollowingCommandHandler(
        IUserRepository userRepository,
        IFollowerService followerService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _followerService = followerService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        User? followed = await _userRepository.GetByIdAsync(command.FollowedId, cancellationToken);

        if (followed is null)
        {
            return Result.Failure(UserErrors.NotFound(command.FollowedId));
        }

        Result result = await _followerService.StartFollowingAsync(
            user,
            followed,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
