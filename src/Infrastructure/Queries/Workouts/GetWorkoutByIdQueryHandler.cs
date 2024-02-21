using Application.Abstractions.Messaging;
using Application.Workouts.GetById;
using Domain.Workouts;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Workouts;

internal sealed class GetWorkoutByIdQueryHandler : IQueryHandler<GetWorkoutByIdQuery, WorkoutResponse>
{
    private readonly ApplicationReadDbContext _context;

    public GetWorkoutByIdQueryHandler(ApplicationReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<WorkoutResponse>> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
    {
        WorkoutResponse? workout = await _context.Workouts
            .Select(w => new WorkoutResponse
            {
                Id = w.Id,
                Name = w.Name,
                UserId = w.UserId,
                Exercises = w.Exercises.Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    ExerciseType = e.ExerciseType,
                    TargetType = e.TargetType,
                    Distance = e.Distance,
                    Duration = e.Duration
                }).ToList()
            })
            .FirstOrDefaultAsync(w => w.Id == request.WorkoutId, cancellationToken);

        if (workout is null)
        {
            return Result.Failure<WorkoutResponse>(WorkoutErrors.NotFound(request.WorkoutId));
        }

        return workout;
    }
}
