using Moq;
using VoteService.Domain.Entities;
using VoteService.Domain.Interfaces;
using VoteService.Domain.Services;

namespace VoteService.Domain.Test;

public class CommentVoteServiceTests
{
    private readonly Mock<ICommentVoteRepository> _mockRepository;
    private readonly CommentVoteService _commentVoteService;
    public CommentVoteServiceTests()
    {
        _mockRepository = new Mock<ICommentVoteRepository>();
        _commentVoteService = new CommentVoteService(_mockRepository.Object);
    }

    [Fact]
    public async Task ToggleCommentVoteAsync_CreatesVote_WhenNoExistingVote()
    {
        // Arrange
        var userId = "user1";
        var commentId = "comment1";
        var voteType = true;

        _mockRepository.Setup(repo => repo.GetVoteByUserIdAsync(userId, commentId))
            .ReturnsAsync((CommentVote)null);

        // Act
        _commentVoteService.ToggleCommentVoteAsync(userId, commentId, voteType);

        // Assert
        _mockRepository.Verify(repo => repo.AddCommentVoteAsync(It.Is<CommentVote>
            (v => v.UserId == userId && v.CommentId == commentId && v.VoteType == voteType)), Times.Once);
    }

    [Fact]
    public async Task ToggleCommentVoteAsync_UpdateVote_WhenVoteTypeDifferent()
    {
        // Arrange
        var userId = "user1";
        var commentId = "comment1";
        var voteType = true;
        var existingVote = CommentVote.Create(userId, commentId, voteType);

        _mockRepository.Setup(repo => repo.GetVoteByUserIdAsync(userId, commentId))
            .ReturnsAsync(existingVote);

        // Act
        var newVoteType = false;
        _commentVoteService.ToggleCommentVoteAsync(userId, commentId, newVoteType);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateVoteAsync(It.Is<CommentVote>
            (v => v.UserId == userId && v.CommentId == commentId && v.VoteType == newVoteType)));
    }

    [Fact]
    public async Task ToggleCommentVoteAsync_DeleteVote_WhenVoteTypeSame()
    {
        // Arrange
        var userId = "user1";
        var commentId = "comment1";
        var voteType = true;
        var existingVote = CommentVote.Create(userId, commentId, voteType);

        _mockRepository.Setup(repo => repo.GetVoteByUserIdAsync(userId, commentId))
            .ReturnsAsync(existingVote);

        // Act
        _commentVoteService.ToggleCommentVoteAsync(userId, commentId, voteType);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteCommentVoteAsync(existingVote));
    }
}