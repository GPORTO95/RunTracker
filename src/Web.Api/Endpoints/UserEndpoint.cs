using Application.Followers.GetFollowersStats;
using Application.Followers.GetRecentFollowers;
using Application.Followers.StartFollowing;
using Application.Users;
using Application.Users.Create;
using Application.Users.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/users", async (
            CreateUserRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateUserCommand(
                request.Email,
                request.Name,
                request.HasPublicProfile);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        app.MapPost("api/users/{userId}/follow/{followedId}",
            async (Guid userId, Guid followedId, ISender sender, CancellationToken cancellationToken) =>
            {
                Result result = await sender.Send(new StartFollowingCommand(userId, followedId), cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            });

        app.MapGet("api/users/{userId}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(userId);

            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        app.MapGet("api/users/{userId}/follower-stats", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetFollowerStatsQuery(userId);

            Result<FollowerStatsResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        app.MapGet("api/users/{userId}/followers/recent", async (
            Guid userId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRecentFollowersQuery(userId);

            Result<List<FollowerResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
