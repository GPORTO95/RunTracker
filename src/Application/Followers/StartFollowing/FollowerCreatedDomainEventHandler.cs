using Application.Abstractions.Notifications;
using Application.Users;
using Application.Users.GetById;
using Domain.Followers;
using MediatR;
using SharedKernel;

namespace Application.Followers.StartFollowing;

internal sealed class FollowerCreatedDomainEventHandler
    : INotificationHandler<FollowerCreatedDomainEvent>
{
    private readonly ISender _sender;
    private readonly INotificationService _notificationService;

    public FollowerCreatedDomainEventHandler(
        ISender sender,
        INotificationService notificationService)
    {
        _sender = sender;
        _notificationService = notificationService;
    }

    public async Task Handle(FollowerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(notification.UserId);

        Result<UserResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new UserNotFoundException(notification.UserId);
        }

        await _notificationService.SendAsync(
            notification.FollowedId,
            $"{result.Value.Name} just got a new follower",
            cancellationToken);
    }
}
