using Domain.Followers;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Domain.UnitTests.Followes;

public class FollowerServiceTests
{
    private readonly FollowerService _followerService;
    private readonly IFollowerRepository _followerRepositoryMock;
    private static readonly Email Email = Email.Create("test@tes.com").Value;
    private static readonly Name Name = new Name("Full name");
    private static readonly DateTime UtcNow = DateTime.UtcNow;

    public FollowerServiceTests()
    {
        _followerRepositoryMock = Substitute.For<IFollowerRepository>();
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(UtcNow);

        _followerService = new FollowerService(_followerRepositoryMock, dateTimeProvider);
    }

    [Fact]
    public async Task StartFollowingAsync_Should_ReturnError_WhenFollowingSameUser()
    {
        // Arrange
        var user = User.Create(Email, Name, false);

        // Act
        var result = await _followerService.StartFollowingAsync(user, user, default);

        // Assert
        result.Error.Should().Be(FollowerErrors.SameUser);
    }

    [Fact]
    public async Task StartFollowingAsync_Should_ReturnError_WhenFollowingNonPublicProfile()
    {
        // Arrange
        var user = User.Create(Email, Name, false);
        var followed = User.Create(Email, Name, false);

        // Act
        var result = await _followerService.StartFollowingAsync(followed, followed, default);

        // Assert
        result.Error.Should().Be(FollowerErrors.NonPublicProfile);
    }

    [Fact]
    public async Task StartFollowingAsync_Should_ReturnError_WhenAlreadyFollowing()
    {
        // Arrange
        var user = User.Create(Email, Name, false);
        var followed = User.Create(Email, Name, true);

        _followerRepositoryMock
            .IsAlreadyFollowingAsync(user.Id, followed.Id, default)
            .Returns(true);

        // Act
        var result = await _followerService.StartFollowingAsync(followed, followed, default);

        // Assert
        result.Error.Should().Be(FollowerErrors.AlreadyFollowing);
    }

    [Fact]
    public async Task StartFollowingAsync_Should_ReturnSuccess_WhenFollowedCreated()
    {
        // Arrange
        var user = User.Create(Email, Name, false);
        var followed = User.Create(Email, Name, true);

        _followerRepositoryMock
            .IsAlreadyFollowingAsync(user.Id, followed.Id, default)
            .Returns(false);

        // Act
        var result = await _followerService.StartFollowingAsync(user, followed, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task StartFollowingAsync_Should_CallInsertOnRepository_WhenFollowedCreated()
    {
        // Arrange
        var user = User.Create(Email, Name, false);
        var followed = User.Create(Email, Name, true);

        _followerRepositoryMock
            .IsAlreadyFollowingAsync(user.Id, followed.Id, default)
            .Returns(false);

        // Act
        await _followerService.StartFollowingAsync(followed, followed, default);

        // Assert
        _followerRepositoryMock.Received(1)
            .Insert(Arg.Is<Follower>(f => f.UserId == user.Id &&
                f.FollowedId == followed.Id &&
                f.CreatedOnUtc == UtcNow));
    }

}
