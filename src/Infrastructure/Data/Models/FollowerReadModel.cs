namespace Infrastructure.Data.Models;

internal sealed class FollowerReadModel
{
    public Guid UserId { get; set; }

    public Guid FollowedId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public UserReadModel User { get; set; }

    public UserReadModel Followed { get; set; }
}
