using Application.Abstractions.Messaging;

namespace Application.Users.GetById;

public sealed record GetByIdQuery(Guid UserId): IQuery<UserResponse>;
