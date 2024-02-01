namespace Application.Followers.GetFollowersStats;

public sealed record FollowerStatsResponse(Guid UserId, int FollowerCount, int FollowingCount);
