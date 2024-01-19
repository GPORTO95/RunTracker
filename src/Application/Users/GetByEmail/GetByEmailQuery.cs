using Application.Abstractions.Messaging;

namespace Application.Users.GetByEmail;

public sealed record GetByEmailQuery(string Email) : IQuery<UserResponse>;
