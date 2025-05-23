﻿using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Contracts;
using Api.FunctionalTests.Extensions;
using FluentAssertions;
using Modules.Users.Application.Users;
using Modules.Users.Application.Users.Create;

namespace Api.FunctionalTests.Users;

public class CreateUserTests : BaseFunctionalTest
{
    public CreateUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEmailIsMissing()
    {
        // Arrange
        var request = new CreateUserRequest("", "name", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        //problemDetails.Errors.Select(e => e.Code
        //    .Should()
        //    .Contain("", UserErrorCodes.CreateUser.MissingEmail, UserErrorCodes.CreateUser.InvalidEmail));
        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([
                UserErrorCodes.CreateUser.MissingEmail,
                UserErrorCodes.CreateUser.InvalidEmail
            ]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new CreateUserRequest("test", "name", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([UserErrorCodes.CreateUser.InvalidEmail]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenNameIsMissing()
    {
        // Arrange
        var request = new CreateUserRequest("test@test.com", "", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code
            .Should()
            .Contain("", UserErrorCodes.CreateUser.MissingName));
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenRequstIsValid()
    {
        // Arrange
        var request = new CreateUserRequest("test@test.com", "name", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Guid userId = await response.Content.ReadFromJsonAsync<Guid>();

        userId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnConflict_WhenUserExists()
    {
        // Arrange
        var request = new CreateUserRequest("test-conflict@test.com", "name", true);

        await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
