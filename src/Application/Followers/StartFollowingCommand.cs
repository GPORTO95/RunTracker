using Application.Abstractions.Messaging;

namespace Application.Followers;


public sealed record StartFollowingCommand(Guid UserId, Guid FollowedId): ICommand;
