using FluentValidation;

namespace Application.Workouts.Create;

internal sealed class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty().WithErrorCode(WorkoutErrorCodes.Create.MissingUserId);

        RuleFor(c => c.Name)
            .NotEmpty().WithErrorCode(WorkoutErrorCodes.Create.MissingName)
            .MaximumLength(100).WithErrorCode(WorkoutErrorCodes.Create.NameTooLong);
    }
}
