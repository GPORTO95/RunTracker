using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationWriteDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationWriteDbContext>();

        dbContext.Database.Migrate();
    }
}
