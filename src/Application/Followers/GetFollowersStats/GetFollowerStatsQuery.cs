using Application.Abstractions.Messaging;

namespace Application.Followers.GetFollowersStats;

public sealed record GetFollowerStatsQuery(Guid UserId) : IQuery<FollowerStatsResponse>;
