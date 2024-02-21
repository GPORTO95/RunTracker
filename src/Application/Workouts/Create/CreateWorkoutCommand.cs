using Application.Abstractions.Messaging;

namespace Application.Workouts.Create;

public sealed record CreateWorkoutRequest(Guid UserId, string Name);
