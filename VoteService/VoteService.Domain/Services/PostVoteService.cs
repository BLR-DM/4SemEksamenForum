using VoteService.Domain.Entities;
using VoteService.Domain.Enums;
using VoteService.Domain.Interfaces;

namespace VoteService.Domain.Services;

public class PostVoteService
{
    private readonly IPostVoteRepository _repository;

    public PostVoteService(IPostVoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<VoteAction> TogglePostVoteAsync(string userId, string postId, bool voteType)
    {
        var existingVote = await _repository.GetVoteByUserIdAsync(userId, postId);

        if (existingVote == null)
        {
            var newVote = PostVote.Create(userId, postId, voteType);
            await _repository.AddPostVoteAsync(newVote);
            return VoteAction.Created;
        }
        else if (existingVote.VoteType == voteType)
        {
            await _repository.DeletePostVoteAsync(existingVote);
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