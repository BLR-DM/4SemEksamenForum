using VoteService.Application.Queries.QueryDto.CommentVote;

namespace VoteService.Application.Queries.Interfaces;

public interface ICommentVoteQuery
{
    Task<GetCommentVoteDto> GetVotesByCommentIdAsync(string commentId);
    Task<List<GetCommentVoteDto>> GetVotesByCommentIdsAsync(IEnumerable<string> commentIds);
}