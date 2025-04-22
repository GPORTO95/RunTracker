using Application.Abstractions.Messaging;

namespace Modules.Users.Application.Users.GetByEmail;

public sealed record GetByEmailQuery(string Email) : IQuery<UserResponse>;
