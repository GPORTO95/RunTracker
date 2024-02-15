using Testcontainers.MsSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Api.FunctionalTests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationWriteDbContext>));

            services.AddDbContext<ApplicationWriteDbContext>(options =>
                options
                    .UseSqlServer(_msSqlContainer.GetConnectionString()));

            services.RemoveAll(typeof(DbContextOptions<ApplicationReadDbContext>));

            services.AddDbContext<ApplicationReadDbContext>(options =>
                options
                    .UseSqlServer(_msSqlContainer.GetConnectionString()));
        });
    }

    public Task InitializeAsync()
        => _msSqlContainer.StartAsync();

    public Task DisposeAsync()
        => _msSqlContainer.DisposeAsync().AsTask();
}
