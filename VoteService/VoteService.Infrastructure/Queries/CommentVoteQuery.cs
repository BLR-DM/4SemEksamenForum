using Microsoft.EntityFrameworkCore;
using VoteService.Application.Queries.Interfaces;
using VoteService.Application.Queries.QueryDto.CommentVote;

namespace VoteService.Infrastructure.Queries;

public class CommentVoteQuery : ICommentVoteQuery
{
    private readonly VoteContext _context;

    public CommentVoteQuery(VoteContext context)
    {
        _context = context;
    }
    async Task<GetCommentVoteDto> ICommentVoteQuery.GetVotesByCommentIdAsync(string commentId)
    {
        var commentVotes = await _context.CommentVotes
            .AsNoTracking()
            .Where(cv => cv.CommentId == commentId)
            .Select(cv => new CommentVoteDto
            {
                UserId = cv.UserId,
                VoteType = cv.VoteType
            }).ToListAsync();

        return new GetCommentVoteDto
        {
            CommentId = commentId,
            CommentVotes = commentVotes
        };
    }

    async Task<List<GetCommentVoteDto>> ICommentVoteQuery.GetVotesByCommentIdsAsync(IEnumerable<string> commentIds)
    {
        var commentVoteList = await _context.CommentVotes
            .AsNoTracking()
            .Where(cv => commentIds.Contains(cv.CommentId))
            .GroupBy(cv => cv.CommentId)
            .Select(g => new GetCommentVoteDto
            {
                CommentId = g.Key,
                CommentVotes = g.Select(cv => new CommentVoteDto
                {
                    UserId = cv.UserId,
                    VoteType = cv.VoteType
                }).ToList()
            }).ToListAsync();

        return commentVoteList;
    }
}