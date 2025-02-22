using Application.Workouts.AddExercises;
using Application.Workouts.Create;
using Application.Workouts.GetById;
using Application.Workouts.Remove;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints;

public static class WorkoutEndpoints
{
    public static void MapWorkoutEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("workouts", async (
            CreateWorkoutRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateWorkoutCommand(request.UserId, request.Name);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        app.MapGet("workouts/{workoutId}", async (
            Guid workoutId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWorkoutByIdQuery(workoutId);

            Result<WorkoutResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        app.MapDelete("workouts/{workoutId}", async (
            Guid workoutId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new RemoveWorkoutCommand(workoutId);

            Result result = await sender.Send(query, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });

        app.MapPost("workouts/{workoutId}/exercises", async (
            Guid workoutId,
            List<ExerciseRequest> exercises,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new AddExercisesCommand(workoutId, exercises);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
