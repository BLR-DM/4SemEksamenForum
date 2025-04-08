using VoteService.Domain.Entities;

namespace VoteService.Domain.Interfaces;

public interface IPostVoteRepository
{
    Task<PostVote?> GetVoteByUserIdAsync(string userId, string postId);
    Task AddPostVoteAsync(PostVote postVote);
    Task UpdateVoteAsync(PostVote postVote);
    Task DeletePostVoteAsync(PostVote postVote);
}