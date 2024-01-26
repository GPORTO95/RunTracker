using Domain.Followers;

namespace Infrastructure.Repositories;

internal sealed class FollowerRepository : IFollowerRepository
{
    public Task<bool> IsAlreadyFollowingAsync(Guid userId, Guid followedId, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }

    public void Insert(Follower follower)
    {
        throw new NotImplementedException();
    }
}
