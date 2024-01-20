﻿using Application.Abstractions.Data;
using Application.Followers;
using Domain.Followers;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Followers;

public class StartFollowingCommandTests
{
    private static readonly User User = User.Create(
        Email.Create("test@test.com").Value,
        new Name("FullName"),
        true);

    private static readonly StartFollowingCommand Command = new(Guid.NewGuid(), Guid.NewGuid());

    private readonly StartFollowingCommandHandler _handler;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IFollowerService _followerServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public StartFollowingCommandTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _followerServiceMock = Substitute.For<IFollowerService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new(_userRepositoryMock, _followerServiceMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenUserIsNull()
    {
        // Arrange
        _userRepositoryMock.GetByIdAsync(Command.UserId).Returns((User?)null);

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(UserErrors.NotFound(Command.UserId));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenFollowedIsNull()
    {
        // Arrange
        _userRepositoryMock.GetByIdAsync(Command.UserId).Returns(User);
        _userRepositoryMock.GetByIdAsync(Command.FollowedId).Returns((User?)null);

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(UserErrors.NotFound(Command.FollowedId));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenStartFollowingFails()
    {
        // Arrange
        _userRepositoryMock.GetByIdAsync(Command.UserId).Returns(User);
        _userRepositoryMock.GetByIdAsync(Command.FollowedId).Returns(User);

        _followerServiceMock.StartFollowingAsync(User, User, default)
            .Returns(FollowerErrors.SameUser);

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(FollowerErrors.SameUser);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenStartFollowingDoesNotFail()
    {
        // Arrange
        _userRepositoryMock.GetByIdAsync(Command.UserId).Returns(User);
        _userRepositoryMock.GetByIdAsync(Command.FollowedId).Returns(User);

        _followerServiceMock.StartFollowingAsync(User, User, default)
            .Returns(Result.Success());

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWork_WhenStartFollowingDoesNotFail()
    {
        // Arrange
        _userRepositoryMock.GetByIdAsync(Command.UserId).Returns(User);
        _userRepositoryMock.GetByIdAsync(Command.FollowedId).Returns(User);

        _followerServiceMock.StartFollowingAsync(User, User, default)
            .Returns(Result.Success());

        // Act
        await _handler.Handle(Command, default);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
