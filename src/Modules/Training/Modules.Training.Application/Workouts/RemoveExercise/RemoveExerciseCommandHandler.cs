﻿using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Modules.Training.Domain.Workouts;
using SharedKernel;

namespace Modules.Training.Application.Workouts.RemoveExercise;

internal sealed class RemoveExerciseCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveExerciseCommand>
{
    public async Task<Result> Handle(RemoveExerciseCommand request, CancellationToken cancellationToken)
    {
        Workout? workout = await workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout is null)
        {
            return Result.Failure(WorkoutErrors.NotFound(request.WorkoutId));
        }

        Result result = workout.RemoveExercise(request.ExerciseId);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

