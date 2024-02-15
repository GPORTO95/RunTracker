using FluentValidation;

namespace Application.Users.Create;

internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithErrorCode(UserErrorCodes.CreateUser.MissingName);

        RuleFor(c => c.Email)
            .NotEmpty().WithErrorCode(UserErrorCodes.CreateUser.MissingEmail)
            .EmailAddress().WithErrorCode(UserErrorCodes.CreateUser.InvalidEmail);
    }
}
