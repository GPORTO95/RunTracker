using Application.Abstractions.Data;
using Application.Users.Create;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users;

public class CreateUserCommandTests
{
    private static readonly CreateUserCommand Command = new ("test@test.com", "FullName", true);

    private readonly CreateUserCommandHandler _handler;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new(_userRepositoryMock, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsInvalid()
    {
        // Arrange
        CreateUserCommand invalidCommand = Command with
        {
            Email = "invalid_email"
        };

        // Act
        Result result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }
}
