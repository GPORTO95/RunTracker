using Application.Abstractions.Messaging;
using Application.Followers.GetRecentFollowers;
using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Followers;

internal sealed class GetRecentFollowersQueryHandler : IQueryHandler<GetRecentFollowersQuery, List<FollowerResponse>>
{
    private readonly ApplicationReadDbContext _context;

    public GetRecentFollowersQueryHandler(ApplicationReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<FollowerResponse>>> Handle(GetRecentFollowersQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
        {
            return Result.Failure<List<FollowerResponse>>(UserErrors.NotFound(request.UserId));
        }

        List<FollowerResponse> followers = await _context.Followers
            .Where(f => f.FollowedId == request.UserId)
            .OrderByDescending(f => f.CreatedOnUtc)
            .Take(10)
            .Select(f => new FollowerResponse(f.User.Id, f.User.Name))
            .ToListAsync(cancellationToken);

        return followers;
    }
}
