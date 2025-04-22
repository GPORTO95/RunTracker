using Application.Abstractions.Data;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace Application.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.0")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDBConnectionFactory));
            services.AddSingleton<IDBConnectionFactory>(_ =>
                new DbConnectionFactory(_msSqlContainer.GetConnectionString()));

            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(_msSqlContainer.GetConnectionString()));

            //services.RemoveAll(typeof(DbContextOptions<ApplicationReadDbContext>));

            //services.AddDbContext<ApplicationReadDbContext>(options =>
            //    options
            //        .UseSqlServer(_msSqlContainer.GetConnectionString()));

            services.RemoveAll(typeof(RedisCacheOptions));
            services.AddStackExchangeRedisCache(redisCacheOptions =>
                redisCacheOptions.Configuration = _redisContainer.GetConnectionString());
        });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    { 
        await _msSqlContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}
