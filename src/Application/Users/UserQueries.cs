using System.Data;
using Application.Abstractions.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Users;

public static class UserQueries
{
    public static async Task<UserDto> GetByIdAsync(
        Guid id,
        IDbConnection connection)
    {
        return await connection.QuerySingleAsync<UserDto>(
            """
            SELECT Id, Name
            FROM Users
            WHERE Id = @UserId
            """,
            new { UserId = id});
    }

    public static async Task<UserDto> GetByIdAsyncEf(
        this IApplicationDbContext dbContext,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto(u.Id, u.Name.Value))
            .SingleAsync(cancellationToken);
    }
}
