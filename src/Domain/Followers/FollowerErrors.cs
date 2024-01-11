using Domain.Abstractions;

namespace Domain.Followers;

public static class FollowerErrors
{
    public static Error SameUser = new("Followers.SameUser", "Can't follow yourself");

    public static Error NonPublicProfile = new("Followers.NonPublicProfile", "Can't follow non-public profile");

    public static Error AlreadyFollowing = new("Followers.AlreadyFollowing", "Already following");
}
