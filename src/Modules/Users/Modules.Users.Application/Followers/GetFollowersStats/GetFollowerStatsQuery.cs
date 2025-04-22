using Application.Abstractions.Messaging;

namespace Modules.Users.Application.Followers.GetFollowersStats;

public sealed record GetFollowerStatsQuery(Guid UserId) : IQuery<FollowerStatsResponse>;
