﻿namespace Modules.Training.Domain.Workouts;

public interface IWorkoutRepository
{
    Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    void Insert(Workout workout);

    void Remove(Workout workout);
}
