using Testcontainers.MsSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Application.Abstractions.Data;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Testcontainers.Redis;

namespace Api.FunctionalTests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

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

    public Task InitializeAsync()
        => _msSqlContainer.StartAsync();

    public Task DisposeAsync()
        => _msSqlContainer.DisposeAsync().AsTask();
}
