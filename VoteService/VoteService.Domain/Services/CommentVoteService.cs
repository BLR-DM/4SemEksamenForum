using VoteService.Domain.Entities;
using VoteService.Domain.Enums;
using VoteService.Domain.Interfaces;

namespace VoteService.Domain.Services;

public class CommentVoteService
{
    private readonly ICommentVoteRepository _repository;

    public CommentVoteService(ICommentVoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<VoteAction> ToggleCommentVoteAsync(string userId, int commentId, bool voteType)
    {
        var existingVote = await _repository.GetVoteByUserIdAsync(userId, commentId);

        if (existingVote == null)
        {
            var newVote = CommentVote.Create(userId, commentId, voteType);
            await _repository.AddCommentVoteAsync(newVote);
            return VoteAction.Created;
        }
        else if (existingVote.VoteType == voteType)
        {
            await _repository.DeleteCommentVoteAsync(existingVote);
            return VoteAction.Deleted;
        }
        else
        {
            existingVote.Update(voteType);
            await _repository.UpdateVoteAsync(existingVote);
            return VoteAction.Updated;
        }
    }
}