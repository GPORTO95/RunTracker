using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, UserResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserResponse>> Handle(GetByIdQuery query, CancellationToken cancellationToken)
    {
        UserResponse? user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == query.UserId)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email.Value,
                Name = u.Name.Value,
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
