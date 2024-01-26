using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Notifications;
using Application.Users;
using Domain.Followers;
using MediatR;

namespace Application.Followers.StartFollowing;

internal sealed class FollowerCreatedDomainEventHandler
    : INotificationHandler<FollowerCreatedDomainEvent>
{
    private readonly INotificationService _notificationService;
    private readonly IApplicationDbContext _dbContext;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public FollowerCreatedDomainEventHandler(
        INotificationService notificationService,
        IApplicationDbContext dbContext,
        IDbConnectionFactory dbConnectionFactory)
    {
        _notificationService = notificationService;
        _dbContext = dbContext;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Handle(FollowerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // STEP 1: Dapper
        using IDbConnection connection = _dbConnectionFactory.CreateOpenConnection();

        UserDto user = await UserQueries.GetByIdAsync(notification.UserId, connection);

        // STEP 2: EF Core
        // UserDto user = await _dbContext.GetByIdAsyncEf(notification.UserId, cancellationToken);

        await _notificationService.SendAsync(
            notification.FollowedId,
            $"{user.Name} just got a new follower",
            cancellationToken);
    }
}
