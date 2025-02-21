using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Abstractions;
using Application.Users;
using Application.Users.Create;
using FluentAssertions;

namespace Api.FunctionalTests.Users;

public class GetUserTests : BaseFunctionalTest
{
    public GetUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnOk_And_User_WhenUserDoesExist()
    {
        // Arrange
        Guid userId = await CreateUserAsync();

        // Act
        UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"api/users/{userId}");

        // Assert
        user.Should().NotBeNull();
    }

    private async Task<Guid> CreateUserAsync()
    {
        var request = new CreateUserRequest("test@test.com", "name", true);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/users", request);

        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}
