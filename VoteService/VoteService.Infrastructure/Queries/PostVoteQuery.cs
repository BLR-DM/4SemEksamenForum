using Microsoft.EntityFrameworkCore;
using VoteService.Application.Queries.Interfaces;
using VoteService.Application.Queries.QueryDto;
using VoteService.Application.Queries.QueryDto.PostVote;

namespace VoteService.Infrastructure.Queries;

public class PostVoteQuery : IPostVoteQuery
{
    private readonly VoteContext _context;

    public PostVoteQuery(VoteContext context)
    {
        _context = context;
    }
    async Task<GetPostVotesDto> IPostVoteQuery.GetVotesByPostIdAsync(string postId)
    {
        var postVotes = await _context.PostVotes
            .AsNoTracking()
            .Where(pv => pv.PostId == postId)
            .Select(pv => new PostVoteDto
            {
                UserId = pv.UserId,
                VoteType = pv.VoteType
            }).ToListAsync();

        return new GetPostVotesDto
        {
            PostId = postId,
            PostVotes = postVotes
        };
    }

    async Task<List<GetPostVotesDto>> IPostVoteQuery.GetVotesByPostIdsAsync(IEnumerable<string> postIds)
    {
        var postVotesList = await _context.PostVotes
            .AsNoTracking()
            .Where(pv => postIds.Contains(pv.PostId)) 
            .GroupBy(pv => pv.PostId)  
            .Select(g => new GetPostVotesDto
            {
                PostId = g.Key,  
                PostVotes = g.Select(pv => new PostVoteDto
                {
                    UserId = pv.UserId,
                    VoteType = pv.VoteType  
                }).ToList()
            })
            .ToListAsync();

        return postVotesList; 
    }
}