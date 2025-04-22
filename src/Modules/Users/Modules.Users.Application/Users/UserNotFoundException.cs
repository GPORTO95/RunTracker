namespace Modules.Users.Application.Users;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId)
        : base($"The user with the identifier {userId} was not found")
    { }
}
