using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly ApplicationReadDbContext _dbContext;

    public GetUserByIdQueryHandler(ApplicationReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        UserResponse? user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == query.UserId)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                HasPublicProfile = u.HasPublicProfile
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        return user;
    }
}
