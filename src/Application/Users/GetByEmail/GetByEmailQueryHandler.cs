using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Users;
using SharedKernel;
using System.Data;

namespace Application.Users.GetByEmail;

internal sealed class GetByEmailQueryHandler
    : IQueryHandler<GetByEmailQuery, UserResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetByEmailQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<UserResponse>> Handle(GetByEmailQuery query, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateOpenConnection();

        const string sql = 
            """
            SELECT u.Id, u.Email, u.Name, u.HasPublicProfile
            FROM Users u
            WHERE u.Email = @Email
            """;

        UserResponse? user = await connection.QueryFirstOrDefaultAsync(
            sql,
            new { query.Email });

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail(query.Email));
        }

        return user;
    }
}
