using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Modules.Users.Domain.Users;
using SharedKernel;

namespace Modules.Users.Application.Users.GetByEmail;

internal sealed class GetByEmailQueryHandler(IDBConnectionFactory factory)
    : IQueryHandler<GetByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetByEmailQuery query, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                u.id AS Id,
                u.email AS Email,
                u.name AS Name,
                u.hasPublicProfile
            FROM users.Users u
            WHERE u.id = @Email
            """;

        using IDbConnection connection = factory.GetOpenConnection();

        UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(sql, query);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        }

        return user;
    }
}
