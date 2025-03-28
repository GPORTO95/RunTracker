﻿using Bogus;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationWriteDbContext>();
        Faker = new Faker();
    }

    protected ISender Sender { get; }

    protected ApplicationWriteDbContext DbContext { get; }

    protected Faker Faker { get; }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
