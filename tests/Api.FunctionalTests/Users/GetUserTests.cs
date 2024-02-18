using System.Net;
using Api.FunctionalTests.Abstractions;
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
}
