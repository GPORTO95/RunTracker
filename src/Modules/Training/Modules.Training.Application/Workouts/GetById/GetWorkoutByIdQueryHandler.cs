﻿using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Modules.Training.Domain.Workouts;
using SharedKernel;

namespace Modules.Training.Application.Workouts.GetById;

internal sealed class GetWorkoutByIdQueryHandler(IDBConnectionFactory factory)
    : IQueryHandler<GetWorkoutByIdQuery, WorkoutResponse>
{
    public async Task<Result<WorkoutResponse>> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                w.id AS Id,
                w.user_id AS UserId,
                w.name AS Name,
                e.id AS ExerciseId,
                e.exerciseType AS ExerciseType,
                e.targetType AS TargetType,
                e.distance AS Distance,
                e.duration AS Duration
            FROM training.Workouts w
            LEFT JOIN training.Exercises e ON e.workout_id = w.id
            WHERE w.id = @WorkoutId
            """;

        using IDbConnection connection = factory.GetOpenConnection();

        var workouts = new Dictionary<Guid, WorkoutResponse>();
        await connection.QueryAsync<WorkoutResponse, ExerciseResponse?, WorkoutResponse>(
            sql,
            (workoutResponse, exerciseResponse) =>
            {
                if (workouts.TryGetValue(request.WorkoutId, out WorkoutResponse existingWorkout))
                {
                    workoutResponse = existingWorkout;
                }
                else
                {
                    workouts.Add(request.WorkoutId, workoutResponse);
                }

                if (exerciseResponse is not null)
                {
                    workoutResponse.Exercises.Add(exerciseResponse);
                }

                return workoutResponse;
            },
            request,
            splitOn: "ExerciseId");

        if (!workouts.TryGetValue(request.WorkoutId, out WorkoutResponse workout))
        {
            return Result.Failure<WorkoutResponse>(WorkoutErrors.NotFound(request.WorkoutId));
        }

        return workout;
    }
}
