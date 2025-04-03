using Application.Abstractions.Events;

namespace Application.Workouts.Remove;

public record WorkoutRemovedIntegrationEvent(Guid Id, Guid WorkoutId) : IntegrationEvent(Id);
