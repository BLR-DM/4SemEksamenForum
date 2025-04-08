using VoteService.Domain.Entities;
using VoteService.Domain.Interfaces;

namespace VoteService.Domain.Services;

public class CommentVoteService
{
    private readonly ICommentVoteRepository _repository;

    public CommentVoteService(ICommentVoteRepository repository)
    {
        _repository = repository;
    }

    public async Task ToggleCommentVoteAsync(string userId, string commentId, bool voteType)
    {
        var existingVote = await _repository.GetVoteByUserIdAsync(userId, commentId);

        if (existingVote == null)
        {
            var newVote = CommentVote.Create(userId, commentId, voteType);
            await _repository.AddCommentVoteAsync(newVote);
        }
        else if (existingVote.VoteType == voteType)
        {
            await _repository.DeleteCommentVoteAsync(existingVote);
        }
        else
        {
            existingVote.Update(voteType);
            await _repository.UpdateVoteAsync(existingVote);
        }
    }
}