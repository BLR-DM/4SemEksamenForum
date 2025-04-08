using VoteService.Application.Queries.QueryDto.PostVote;

namespace VoteService.Application.Queries.Interfaces;

public interface IPostVoteQuery
{
    Task<GetPostVotesDto> GetVotesByPostIdAsync(string postId);
    Task<List<GetPostVotesDto>> GetVotesByPostIdsAsync(IEnumerable<string> postIds);
}