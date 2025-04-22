using Application.Abstractions.Caching;
using Modules.Users.Application.Users.GetByEmail;

namespace Modules.Users.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : ICachedQuery<UserResponse>
{
    public string CacheKey => $"users-by-id-{UserId}";

    public TimeSpan? Expiration => null;
}
