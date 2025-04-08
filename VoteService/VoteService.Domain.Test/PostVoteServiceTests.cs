using Moq;
using VoteService.Domain.Entities;
using VoteService.Domain.Interfaces;
using VoteService.Domain.Services;

namespace VoteService.Domain.Test;

public class PostVoteServiceTests
{
    private readonly Mock<IPostVoteRepository> _mockRepository;
    private readonly PostVoteService _service;
    public PostVoteServiceTests()
    {
        _mockRepository = new Mock<IPostVoteRepository>();
        _service = new PostVoteService(_mockRepository.Object);
    }

    [Fact]
    public async Task TogglePostVoteAsync_CreatesVote_WhenNoExistingVote()
    {
        // Arrange
        var userId = "user1";
        var postId = "post1";
        var voteType = true;

        _mockRepository.Setup(repo => repo.GetVoteByUserIdAsync(userId, postId))
            .ReturnsAsync((PostVote)null); 

        // Act
        await _service.TogglePostVoteAsync(userId, postId, voteType);

        // Assert
        _mockRepository.Verify(repo => repo.AddPostVoteAsync(It.Is<PostVote>
            (v => v.UserId == userId && v.PostId == postId && v.VoteType == voteType)), Times.Once);
    }

    [Fact]
    public async Task TogglePostVoteAsync_DeletesVote_WhenSameVoteType()
    {
        // Arrange
        var userId = "user1";
        var postId = "post1";
        var voteType = true;
        var existingVote = PostVote.Create(userId, postId, voteType);

        _mockRepository.Setup(repo => repo.GetVoteByUserIdAsync(userId, postId))
            .ReturnsAsync(existingVote);  

        // Act
        await _service.TogglePostVoteAsync(userId, postId, voteType);

        // Assert
        _mockRepository.Verify(repo => repo.DeletePostVoteAsync(existingVote), Times.Once);
    }

    [Fact]
    public async Task TogglePostVoteAsync_UpdatesVote_WhenDifferentVoteType()
    {
        // Arrange
        var userId = "user1";
        var postId = "post1";
        var initialVoteType = true;
        var existingVote = PostVote.Create(userId, postId, initialVoteType);

        _mockRepository.Setup(repo => repo.GetVoteByUserIdAsync(userId, postId))
            .ReturnsAsync(existingVote);  

        // Act
        var newVoteType = false;
        await _service.TogglePostVoteAsync(userId, postId, newVoteType);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateVoteAsync(It.Is<PostVote>
            (v => v.UserId == userId && v.PostId == postId && v.VoteType == newVoteType)), Times.Once);
    }
}