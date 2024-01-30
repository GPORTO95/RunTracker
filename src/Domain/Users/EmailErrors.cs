using SharedKernel;

namespace Domain.Users;

public static class EmailErrors
{
    public static readonly Error Empty = Error.Validation("Email.Empty", "Email is empty");

    public static readonly Error InvalidFormat = Error.Validation("Email.InvalidFormat", "Email format is invalid");
}
