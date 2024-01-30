using SharedKernel;

namespace Domain.Followers;

public static class FollowerErrors
{
    public static readonly Error SameUser = Error.Validation("Followers.SameUser", "Can't follow yourself");

    public static readonly Error NonPublicProfile = Error.Validation("Followers.NonPublicProfile", "Can't follow non-public profile");

    public static readonly Error AlreadyFollowing = Error.Conflict("Followers.AlreadyFollowing", "Already following");
}
