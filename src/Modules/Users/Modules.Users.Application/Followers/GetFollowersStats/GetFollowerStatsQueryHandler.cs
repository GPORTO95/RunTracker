using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Modules.Users.Application.Followers.GetFollowersStats;
using SharedKernel;

namespace Modules.Users.Application.Followers.GetFollowerStats;

internal sealed class GetFollowerStatsQueryHandler(IDBConnectionFactory factory)
    : IQueryHandler<GetFollowerStatsQuery, FollowerStatsResponse>
{
    public async Task<Result<FollowerStatsResponse>> Handle(
        GetFollowerStatsQuery request,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                @UserId AS UserId,
                (
                    SELECT COUNT(*)
                    FROM users.Followers f
                    WHERE f.followed_id = @UserId
                ) AS FollowerCount,
                (
                    SELECT COUNT(*)
                    FROM users.Followers f
                    WHERE f.user_id = @UserId
                ) AS FollowingCount
            """;

        using IDbConnection connection = factory.GetOpenConnection();

        FollowerStatsResponse followerStats = await connection.QueryFirstAsync<FollowerStatsResponse>(sql, request);

        return followerStats;
    }
}
