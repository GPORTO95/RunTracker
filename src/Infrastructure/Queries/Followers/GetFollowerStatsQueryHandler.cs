using Application.Abstractions.Messaging;
using Application.Followers.GetFollowersStats;
using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Followers;

internal sealed class GetFollowerStatsQueryHandler
    : IQueryHandler<GetFollowerStatsQuery, FollowerStatsResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public GetFollowerStatsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<FollowerStatsResponse>> Handle(GetFollowerStatsQuery request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
        {
            return Result.Failure<FollowerStatsResponse>(UserErrors.NotFound(request.UserId));
        }

        int followerCount = await _dbContext.Followers.CountAsync(f => f.FollowedId == request.UserId, cancellationToken);

        int followingCount = await _dbContext.Followers.CountAsync(f => f.UserId ==  request.UserId, cancellationToken);

        return new FollowerStatsResponse(request.UserId, followerCount, followingCount);
    }
}
